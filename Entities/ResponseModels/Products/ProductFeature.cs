using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductFeature
    {
        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;
    }
}
