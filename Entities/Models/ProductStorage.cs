using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class ProductStorage
    {
        public int Id { get; set; }
     
        public int ProductId { get; set; }
        
        public Product? Product { get; set; } = null!;
        
        public int SizeId { get; set; }
        
        public Size? Size { get; set; } = null!;

        [Range(0, Int32.MaxValue, ErrorMessage = "Count must be positive")]
        public int Count { get; set; } = 0;
    }
}
