using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Users
{
    public class UserEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
