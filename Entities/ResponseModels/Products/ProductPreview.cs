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
            Name = product.Name;
            Price = product.Price;
            Brand = new BrandResponse(product.Brand);
            Images = product.ProductImages;
            IsNew = product.IsNew;
            Discount = product.Discount;
            CreatedDate = product.CreatedDate;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Article { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
        
        public bool IsNew { get; set; } = false;
        
        public BrandResponse? Brand { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductColor> Colors { get; set; } = new List<ProductColor>();

        public Discount? Discount { get; set; }
        
        public IEnumerable<ProductSize> Sizes { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
