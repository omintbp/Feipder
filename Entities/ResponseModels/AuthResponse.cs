using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels
{
    public class AuthResponse
    {
        [Required]
        [StringLength(16, MinimumLength =6)]
        public string Username { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;
    }
}
