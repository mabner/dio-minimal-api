using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DataBaseContext _context;

        public VehicleService(DataBaseContext context)
        {
            _context = context;
        }

        public Vehicle Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return vehicle;
        }

        public Vehicle Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();

            return vehicle;
        }

        public Vehicle Remove(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();

            return vehicle;
        }

        public Vehicle? GetVehicleById(int id)
        {
            return _context.Vehicles.Find(id);
        }

        public List<Vehicle> GetVehicles(int? page = 1, string? model = null, string? make = null)
        {
            var query = _context.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model))
            {
                // TODO: #1 Validate Like syntax (?$"{model}")
                query = query.Where(v => EF.Functions.Like(v.Model.ToLower(), model.ToLower()));
            }

            int itensPerPage = 10;

            if (page != null)
            {
                query = query.Skip(((int)page - 1) * itensPerPage).Take(itensPerPage);
            }

            return query.ToList();
        }
    }
}
