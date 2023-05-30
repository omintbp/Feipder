using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Users
{
    public class AdmUserPut
    {
        [Required]
        public Guid Id { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50, MinimumLength = 1)]
        public string? Phone { get; set; }
    }
}
