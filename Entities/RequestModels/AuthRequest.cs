using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class AuthRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;
    }
}
