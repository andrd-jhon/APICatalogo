using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Services;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id", user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAcessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO registerModelDTO)
        {
            var userExists = await _userManager.FindByNameAsync(registerModelDTO.UserName!);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new()
            {
                Email = registerModelDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModelDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, registerModelDTO.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed." });
            }

            return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("refresh-tokken")]
        public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModelDTO)
        {
            if (tokenModelDTO is null)
                return BadRequest("Invalid client request");

            string? acessToken = tokenModelDTO.AccessToken ?? throw new ArgumentException(nameof(tokenModelDTO));

            string? refreshToken = tokenModelDTO.RefreshToken ?? throw new ArgumentException(nameof(tokenModelDTO));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken, _configuration);

            if (principal == null)
                return BadRequest("Invalid acess token/refresh token");

            string userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName!);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid access token/refresh token");

            var newAccessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
                return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("CreateRole")]
        //[Authorize("SuperAdminOnly")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");

                    return StatusCode(StatusCodes.Status200OK, new ResponseDTO
                    {
                        Status = "Success",
                        Message = $"Role {roleName} added successfully"
                    });
                }
                else
                {
                    _logger.LogInformation(2, "Error");

                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
                    {
                        Status = "Error",
                        Message = $"Issue while adding new {roleName} role"
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
            {
                Status = "Error",
                Message = $"Role {roleName} already exists"
            });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        //[Authorize("SuperAdminOnly")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user} added to the {roleName} role");

                    return StatusCode(StatusCodes.Status200OK, new ResponseDTO
                    {
                        Status = "Success",
                        Message = $"User {user.Email} added to the {roleName} role"
                    });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: unable to add user {user.Email} to the {roleName} role");

                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
                    {
                        Status = "Error",
                        Message = $"Error: unable to add user {user.Email} to the {roleName} role"
                    });
                }
            }

            return BadRequest(new { Error = "Unable to find user" });
        }
    }
}
