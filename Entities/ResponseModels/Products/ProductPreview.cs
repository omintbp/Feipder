using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductPreview
    {
        public ProductPreview(Product product) 
        { 
            Id = product.Id;
            Article = product.Article;
            Name = product.Alias;
            Price = product.Price;
            Brand = new BrandResponse(product.Brand);
            PreviewImage = product.PreviewImage;
            NewProduct = product.IsNew;
            Discount = product.Discount;
            CreatedDate = product.CreatedDate;
            Color = product.Color;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Article { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
        
        public bool NewProduct { get; set; } = false;
        
        public BrandResponse? Brand { get; set; }
        
        public ProductPreviewImage? PreviewImage { get; set; } = null!;
        
        public Color? Color { get; set; }
        
        public Discount? Discount { get; set; }
        
        public IEnumerable<ProductSize> Sizes { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
