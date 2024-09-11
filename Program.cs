using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enums;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

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
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
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
).WithTags("Administrators");

app.MapPost(
    "/administrators",
    ([FromBody] AdministratorDTO administratorDTO, IAdministratorService administratorService) =>
    {
        var validation = new ValidationErrors
        {
            Messages = new List<string>()
        };
        if (string.IsNullOrEmpty(administratorDTO.Email))
            validation.Messages.Add("E-mail must be informed.");
        if (string.IsNullOrEmpty(administratorDTO.Password))
            validation.Messages.Add("Password must be informed.");
        if (administratorDTO.Profile == null)
            validation.Messages.Add("Profile must be informed.");

        if (validation.Messages.Count > 0)
            return Results.BadRequest(validation);

        var administrator = new Administrator
        {
            Email = administratorDTO.Email,
            Password = administratorDTO.Password,
            Profie = administratorDTO.Profile?.ToString() ?? Profile.user.ToString(),
        };


        return Results.Created($"/administrator/{administrator.Id}", administrator);
    }
).WithTags("Administrators");

app.MapGet(
    "/administrators",
    ([FromQuery] int? page, IAdministratorService administratorService) =>
    {
        return Results.Ok(administratorService.GetAdministrators(page));
    }
).WithTags("Administrators");
#endregion

#region Vehicle
ValidationErrors validateDTO(VehicleDTO vehicleDTO)
{
    var validation = new ValidationErrors
    {
        Messages = new List<string> { }
    };

    if (string.IsNullOrEmpty(vehicleDTO.Model))
        validation.Messages.Add("Model must be informed.");
    if (string.IsNullOrEmpty(vehicleDTO.Make))
        validation.Messages.Add("Make must be informed.");
    if (vehicleDTO.Year == 0)
        validation.Messages.Add("Year of manufacture must be informed.");

    return validation;
}

app.MapPost(
    "/vehicles",
    ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
    {
        var validation = validateDTO(vehicleDTO);
        if (validation.Messages.Count > 0)
            return Results.BadRequest(validation);

        var vehicle = new Vehicle
        {
            Model = vehicleDTO.Model,
            Make = vehicleDTO.Make,
            Year = vehicleDTO.Year,
        };
        vehicleService.Add(vehicle);

        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }
).WithTags("Vehicle");

app.MapGet(
    "/vehicles",
    ([FromQuery] int? page, IVehicleService vehicleService) =>
    {
        var vehicles = vehicleService.GetVehicles(page);

        return Results.Ok(vehicles);
    }
).WithTags("Vehicle");

app.MapGet(
    "/vehicle/{id}",
    ([FromRoute] int id, IVehicleService vehicleService) =>
    {
        var vehicle = vehicleService.GetVehicleById(id);

        if (vehicle == null) return Results.NotFound();
        return Results.Ok(vehicle);
    }
).WithTags("Vehicle");

app.MapPut(
    "/vehicle/{id}",
    ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
    {
        var vehicle = vehicleService.GetVehicleById(id);
        if (vehicle == null) return Results.NotFound();

        var validation = validateDTO(vehicleDTO);
        if (validation.Messages.Count > 0)
            return Results.BadRequest(validation);

        vehicle.Model = vehicleDTO.Model;
        vehicle.Make = vehicleDTO.Make;
        vehicle.Year = vehicleDTO.Year;

        vehicleService.Update(vehicle);

        return Results.Ok(vehicle);
    }
).WithTags("Vehicle");

app.MapDelete(
    "/vehicle/{id}",
    ([FromRoute] int id, IVehicleService vehicleService) =>
    {
        var vehicle = vehicleService.GetVehicleById(id);

        if (vehicle == null) return Results.NotFound();
        vehicleService.Remove(vehicle);

        return Results.NoContent();
    }
).WithTags("Vehicle");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "DIO - Minimal API V1");
});

app.Run();
#endregion