using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models.ResponseModels.Products
{
    public class ProductResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = null!;
        
        [Required]
        [StringLength(40)]
        public string Article { get; set; } = null!;

        [Required]
        [Range(0, Double.MaxValue)]
        public double Price { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = null!;

        public bool IsNew { get; set; } = false;

        public Color? Color { get; set; }
        public BrandResponse? Brand { get; set; }
        public Category? Category { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
