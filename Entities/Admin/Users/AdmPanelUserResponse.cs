using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Users
{
    public class AdmPanelUserResponse
    {
        public Guid Id { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }

        public DateTimeOffset UpdateDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
    }
}
