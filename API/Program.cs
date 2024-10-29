using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enums;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key))
    key = "123456";

builder
    .Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DIO - Minimal API", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insert the JWT token:",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
        }
    );
});

// Passing the config with the builder
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new HomeModelView())).AllowAnonymous().WithTags("Home");
#endregion

#region Administrators

string NewJwtToken(Administrator administrator)
{
    if (string.IsNullOrEmpty(key))
        return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrator.Email),
        new Claim("Profile", administrator.Profie),
        new Claim(ClaimTypes.Role, administrator.Profie),
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost(
        "/administrators/login",
        ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
        {
            var admin = administratorService.Login(loginDTO);
            if (admin != null)
            {
                string token = NewJwtToken(admin);
                return Results.Ok(
                    new LoggedAdminModelView
                    {
                        Email = admin.Email,
                        Profile = admin.Profie,
                        Token = token,
                    }
                );
            }
            else
                return Results.Unauthorized();
        }
    )
    .AllowAnonymous()
    .WithTags("Administrators");

app.MapPost(
        "/administrators",
        (
            [FromBody] AdministratorDTO administratorDTO,
            IAdministratorService administratorService
        ) =>
        {
            var validation = new ValidationErrorsModelView { Messages = new List<string>() };
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
                Profie = administratorDTO.Profile.ToString() ?? Profile.User.ToString(),
            };

            administratorService.Add(administrator);

            return Results.Created(
                $"/administrators/{administrator.Id}",
                new AdministratorModelView
                {
                    Id = administrator.Id,
                    Email = administrator.Email,
                    Profile = administrator.Profie
                }
            );
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
    .WithTags("Administrators");

app.MapGet(
        "/administrators",
        ([FromQuery] int? page, IAdministratorService administratorService) =>
        {
            var admins = new List<AdministratorModelView>();
            var administrators = administratorService.GetAdministrators(page);
            foreach (var admin in administrators)
            {
                admins.Add(
                    new AdministratorModelView
                    {
                        Id = admin.Id,
                        Email = admin.Email,
                        Profile = admin.Profie
                    }
                );
            }
            return Results.Ok(admins);
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
    .WithTags("Administrators");

app.MapGet(
        "/administrators/{id}",
        ([FromRoute] int id, IAdministratorService administratorService) =>
        {
            var administrator = administratorService.GetAdministratorById(id);

            if (administrator == null)
                return Results.NotFound();
            return Results.Ok(
                new AdministratorModelView
                {
                    Id = administrator.Id,
                    Email = administrator.Email,
                    Profile = administrator.Profie
                }
            );
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
    .WithTags("Administrators");

#endregion

#region Vehicle
ValidationErrorsModelView validateDTO(VehicleDTO vehicleDTO)
{
    var validation = new ValidationErrorsModelView { Messages = new List<string> { } };

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
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Superuser" })
    .WithTags("Vehicle");

app.MapGet(
        "/vehicles",
        ([FromQuery] int? page, IVehicleService vehicleService) =>
        {
            var vehicles = vehicleService.GetVehicles(page);

            return Results.Ok(vehicles);
        }
    )
    .RequireAuthorization()
    .WithTags("Vehicle");

app.MapGet(
        "/vehicle/{id}",
        ([FromRoute] int id, IVehicleService vehicleService) =>
        {
            var vehicle = vehicleService.GetVehicleById(id);

            if (vehicle == null)
                return Results.NotFound();
            return Results.Ok(vehicle);
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Superuser" })
    .WithTags("Vehicle");

app.MapPut(
        "/vehicle/{id}",
        ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
        {
            var vehicle = vehicleService.GetVehicleById(id);
            if (vehicle == null)
                return Results.NotFound();

            var validation = validateDTO(vehicleDTO);
            if (validation.Messages.Count > 0)
                return Results.BadRequest(validation);

            vehicle.Model = vehicleDTO.Model;
            vehicle.Make = vehicleDTO.Make;
            vehicle.Year = vehicleDTO.Year;

            vehicleService.Update(vehicle);

            return Results.Ok(vehicle);
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
    .WithTags("Vehicle");

app.MapDelete(
        "/vehicle/{id}",
        ([FromRoute] int id, IVehicleService vehicleService) =>
        {
            var vehicle = vehicleService.GetVehicleById(id);

            if (vehicle == null)
                return Results.NotFound();
            vehicleService.Remove(vehicle);

            return Results.NoContent();
        }
    )
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
    .WithTags("Vehicle");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "DIO - Minimal API V1");
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
