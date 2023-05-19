using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class RegistrationRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        
        [Required]
        [StringLength(15, ErrorMessage = "Your username is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Username { get; set; } = null!;

        [StringLength(15, MinimumLength = 0)]
        public string? FirstName { get; set; }

        [StringLength(15, MinimumLength = 0)]
        public string? LastName { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
