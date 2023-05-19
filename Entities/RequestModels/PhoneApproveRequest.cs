using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class PhoneApproveRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string ApproveCode { get; set; } = null!;
    }
}
