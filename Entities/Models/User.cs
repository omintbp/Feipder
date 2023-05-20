using Microsoft.AspNetCore.Identity;

namespace Feipder.Entities.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTimeOffset? LastLoginAttempt { get; set; }
        public string? LastCode { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; } = null!;

        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}
