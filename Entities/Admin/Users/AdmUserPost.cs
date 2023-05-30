using AutoFixture;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Users
{
    public class AdmUserPost
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
    }
}
