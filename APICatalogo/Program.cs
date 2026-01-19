using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Controllers, Filters & JSON Settings

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(APIExceptionFilter));
})
.AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
.AddNewtonsoftJson();

#endregion

#region CORS

builder.Services.AddCors(options => options.AddPolicy("OrigensComAcessoPermitido", policy =>
{
    policy
    .WithOrigins("https://localhost:7022")
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

#endregion

#region Swagger / OpenAPI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

#endregion

#region Configuration Values (appSettings.json)

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

#endregion

#region Database

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

#endregion

#region Authentication & Authorization

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options => {

    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));

    options.AddPolicy("OwnerOnly", policy => policy.RequireRole("owner")/*.RequireClaim("id", "teste")*/);

    options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));

    options.AddPolicy("AdminOrOwner", policy => policy.RequireRole("admin", "owner"));

    //options.AddPolicy("ExclusivePolicyOnly", policy =>
    //policy.RequireAssertion(context =>
    //context.User.HasClaim(claim =>
    //claim.Type == "id" && claim.Value == "joao" || context.User.IsInRole("SuperAdmin"))).RequireClaim("id", "macoratti"));
});

#endregion

#region

builder.Services.AddRateLimiter(rateLimiteOptions =>
{
    rateLimiteOptions.AddFixedWindowLimiter(policyName: "fixedWindow", options =>
    {
        options.PermitLimit = 1;
        options.Window = TimeSpan.FromSeconds(5);
        options.QueueLimit = 0;
    });

    rateLimiteOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

#endregion

#region Dependency Injection - Repositories & Unit Of Work

builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

#region Logging

builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information,
        }));

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

#endregion 

var app = builder.Build();

#region Middleware & Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter();

app.UseCors("OrigensComAcessoPermitido");

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();

