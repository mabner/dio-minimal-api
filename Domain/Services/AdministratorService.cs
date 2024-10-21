using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly DataBaseContext _context;

        public AdministratorService(DataBaseContext context)
        {
            _context = context;
        }

        public Administrator? Login(LoginDTO loginDTO)
        {
            var admin = _context.Administrators.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return admin;
        }

        public Administrator Add(Administrator administrator)
        {
            _context.Administrators.Add(administrator);
            _context.SaveChanges();

            return administrator;

        }

        public List<Administrator> GetAdministrators(int? page)
        {
            var query = _context.Administrators.AsQueryable();
            
            int itensPerPage = 10;

            if (page != null)
            {
                query = query.Skip(((int)page - 1) * itensPerPage).Take(itensPerPage);
            }

            return query.ToList();
        }

        public Administrator? GetAdministratorById(int id)
        {
            return _context.Administrators.Find(id);
        }
    }
}
