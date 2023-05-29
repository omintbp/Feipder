using Feipder.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    [BindProperties]
    public class PostOrderRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        public DeliveryType DeliveryType { get; set; }

        public int PickupPointId { get; set; }

        public Address? Address { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
