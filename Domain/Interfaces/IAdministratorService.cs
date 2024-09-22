using MinimalApi.Domain.Entities;
using MinimalApi.Domain.DTOs;

namespace MinimalApi.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);
        Administrator Add(Administrator administrator);
        List<Administrator> GetAdministrators(int? page);
    }
}
