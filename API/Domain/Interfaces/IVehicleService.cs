using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> GetVehicles(int? page = 1, string? model = null, string? make = null);
        Vehicle? GetVehicleById(int id);

        Vehicle? Add(Vehicle vehicle);

        Vehicle? Update(Vehicle vehicle);

        Vehicle? Remove(Vehicle vehicle);
    }
}
