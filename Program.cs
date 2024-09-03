using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DIO - Minimal API", Version = "v1" });
});

// Passing the config with the builder
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administrators
app.MapPost(
    "/administrators/login",
    ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
    {
        if (administratorService.Login(loginDTO) != null)
            return Results.Ok("Login com sucesso");
        else
            return Results.Unauthorized();
    }
);
#endregion

#region Vehicle

#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "DIO - Minimal API V1");
});

app.Run();
#endregion