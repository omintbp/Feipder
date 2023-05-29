using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class RegistrationCallRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
