using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }

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

        [Required]
        public OrderStatus OrderStatus { get; set; }

        public Address? Address { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public ICollection<OrderItemResponse> Items = new List<OrderItemResponse>();

        [Range(0, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double ItemsCount { get; set; }
    }
}
