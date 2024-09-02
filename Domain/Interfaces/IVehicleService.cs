using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> GetVehicles(int page = 1, string? name = null, string? make = null);
        Vehicle? GetVehicleById(int id);

        void Add(Vehicle vehicle);

        void Update(Vehicle vehicle);

        void Remove(Vehicle vehicle);
    }
}
