using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using System.Reflection;

namespace Test.Domain.Entities;

[TestClass]
public class VehicleServiceTest
{
    private DataBaseContext CreateTestContext()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DataBaseContext(configuration);
    }

    [TestMethod]
    public void SaveVehicleTest()
    {
        // Arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("truncate table vehicles");

        var vehicle = new Vehicle();
        vehicle.Make = "Volkswagem";
        vehicle.Model = "Beetle";
        vehicle.Year = 2001;

        var vehicleServices = new VehicleService(context);

        // Act (Set)
        vehicleServices.Add(vehicle);

        // Assert (Get)
        Assert.AreEqual(1, vehicleServices.GetVehicles(1).Count());
    }

    [TestMethod]
    public void GetVehicleByIdTest()
    {
        // Arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("truncate table vehicles");

        var vehicle = new Vehicle();
        vehicle.Make = "Chevrolet";
        vehicle.Model = "Chevette";
        vehicle.Year = 1987;

        var vehicleServices = new VehicleService(context);

        // Act (Set)
        vehicleServices.Add(vehicle);
        var vehicleFromDb = vehicleServices.GetVehicleById(vehicle.Id);

        // Assert (Get)
        Assert.AreEqual(1, vehicleFromDb?.Id);
    }
}
