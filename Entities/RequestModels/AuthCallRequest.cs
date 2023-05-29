using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class AuthCallRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
