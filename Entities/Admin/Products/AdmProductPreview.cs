using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Products
{
    public class AdmProductPreview
    {
        public AdmProductPreview(Product product) 
        { 
            Id = product.Id;
            Article = product.Article;
            Name = product.Name;
            Price = product.Price;
            Alias = product.Title;
            Brand = new BrandResponse(product.Brand);
            Category = new CategoryResponse(product.Category);
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
        public string Alias { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public bool IsNew { get; set; } = false;
        
        public BrandResponse? Brand { get; set; }

        public CategoryResponse? Category { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductColor> Colors { get; set; } = new List<ProductColor>();

        public Discount? Discount { get; set; }
        
        public IEnumerable<ProductSize> Sizes { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
