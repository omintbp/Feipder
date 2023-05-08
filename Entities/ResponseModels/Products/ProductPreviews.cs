using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization.Metadata;

namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductPreviews
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int ProductsCount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public double MinPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public double MaxPrice { get; set; }
        
        public ICollection<ProductPreview> Products { get; set; } = new List<ProductPreview>();
        public ICollection<ProductProperty> ProductProperties { get; set; } = new List<ProductProperty>();
    }
}
