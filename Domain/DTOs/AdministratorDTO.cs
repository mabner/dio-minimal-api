using MinimalApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Domain.DTOs
{
    public class AdministratorDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Profile? Profile { get; set; } = default!;
    }
}
