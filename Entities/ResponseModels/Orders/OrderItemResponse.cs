using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Orders
{
    public class OrderItemResponse
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;

        [Required]
        public string Size { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Count { get; set; }
    }
}
