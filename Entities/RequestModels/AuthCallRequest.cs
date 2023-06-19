using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class AuthCallRequest
    {
        [Required]
        [Phone]
        [RegularExpression(@"^((\+7|7)+([0-9]){10})$")]
        public string PhoneNumber { get; set; } = null!;
    }
}
