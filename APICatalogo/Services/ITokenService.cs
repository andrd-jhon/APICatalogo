using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services
{
    public interface ITokenService
    {
        SecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    }
}
