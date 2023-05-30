using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class RegistrationRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        
        [StringLength(15, MinimumLength = 0)]
        public string? FirstName { get; set; }

        [StringLength(15, MinimumLength = 0)]
        public string? LastName { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
