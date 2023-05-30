using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Users
{
    public class UserPhoneRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
    }
}
