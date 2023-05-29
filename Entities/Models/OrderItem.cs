using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
     
        public Product Product { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }
        
        public int SizeId { get; set; }
        public Size Size { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Count { get; set; }

        public Order Order { get; set; } = null!;
    }
}
