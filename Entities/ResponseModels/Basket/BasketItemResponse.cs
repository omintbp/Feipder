using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels.Products;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Basket
{
    public class BasketItemResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Article { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }

        public ProductImage? ProductImage { get; set; } = null!;

        public ProductColor? ProductColor { get; set; } = null!;

        public ProductSize? ProductSize { get; set; } = null!;

        public BrandResponse? Brand { get; set; } = null!;

        public CategoryResponse? Category { get; set; } = null!;

        public bool IsLast { get; set; } = false;

        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Available { get; set; }
    }
}
