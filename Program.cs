using Microsoft.EntityFrameworkCore;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);

// Passing teh config with the builder
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost(
    "/login",
    (LoginDTO loginDTO) =>
    {
        if (loginDTO.Email == "adm@test.com" && loginDTO.Password == "123456")
            return Results.Ok("Login com sucesso");
        else
            return Results.Unauthorized();
    }
);

app.Run();
