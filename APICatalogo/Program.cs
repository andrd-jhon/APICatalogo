using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Logging;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

#endregion

#region Swagger / OpenAPI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Configuration Values (appSettings.json)

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

#endregion

#region Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection,
        ServerVersion.AutoDetect(mySqlConnection)));

#endregion

#region Dependency Injection - Repositories & Unit Of Work

builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

#endregion 

#region API Behavior

builder.Services.Configure<ApiBehaviorOptions>(options => 
{
    options.DisableImplicitFromServicesParameters = true;
});

#endregion

#region Logging

builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information,
        }));

#endregion

#region Authentication & Authorization

builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddAuthorization();

#endregion

var app = builder.Build();

#region Middleware & Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

#endregion

app.Run();

