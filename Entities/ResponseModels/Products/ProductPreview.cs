using Feipder.Entities.Models;

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
            Brand = product.Brand;
            PreviewImages = new List<string>();
            NewProduct = product.IsNew;
            Discount = product.Discount;
            CreatedDate = product.CreatedDate;
            Color = product.Color;
        }

        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool NewProduct { get; set; }
        public Brand Brand { get; set; }
        public List<string> PreviewImages { get; set; } = new List<string>();
        public Color Color { get; set; }
        public Discount Discount { get; set; }
        public IEnumerable<ProductSize> Sizes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
