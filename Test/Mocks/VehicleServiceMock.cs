using MinimalApi.Domain.Interfaces;
using Vehicle = MinimalApi.Domain.Entities.Vehicle;

namespace Test.Mocks;

internal class VehicleServiceMock : IVehicleService
{
    private static List<Vehicle> vehicles = new List<Vehicle>()
    {
        new Vehicle
        {
            Id = 1,
            Make = "Volkswagem",
            Model = "Beetle",
            Year = 2001
        },
        new Vehicle
        {
            Id = 2,
            Make = "Chevrolet",
            Model = "Chevette",
            Year = 1987
        },
    };

    public Vehicle Add(Vehicle vehicle)
    {
        vehicle.Id = vehicles.Count() + 1;
        vehicles.Add(vehicle);

        return vehicle;
    }

    public Vehicle? GetVehicleById(int id)
    {
        return vehicles.Find(v => v.Id == id);
    }

    public List<Vehicle> GetVehicles(int? page = 1, string? model = null, string? make = null)
    {
        return vehicles;
    }

    public Vehicle Remove(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public Vehicle Update(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }
}

