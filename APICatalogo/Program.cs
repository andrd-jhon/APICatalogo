using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

string? valor1 = builder.Configuration["Chave1"];
string? s1Valor2 = builder.Configuration["Secao1:Chave2"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection,
        ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMeuServico, MeuServico>(); /*Esse trecho de codigo indica que toda vez que uma classe
                                                           solicitar essa dependência, ela será instanciada!*/

builder.Services.Configure<ApiBehaviorOptions>(options => /*Esse trecho de codigo indica que a injeção de dependencias explicita nos controlladores
                                                           está desabilitada (impedindo a injeção sem o uso do [FromServices])*/
{
    options.DisableImplicitFromServicesParameters = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    //
    await next(context);
    //
});

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Middleware final!");
//});

app.MapControllers();

app.Run();
