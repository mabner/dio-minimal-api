using MinimalApi.Domain.Entities;
using MinimalApi.Domain.DTOs;

namespace MinimalApi.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);
    }
}
