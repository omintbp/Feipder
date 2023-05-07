namespace Feipder.Entities.Models.ResponseModels.Products
{
    public class ProductResponse
    {
        public class ProductCategory
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Alias { get; set; }
            public string? Image { get; set; }
            public bool IsVisible { get; set; }
        }

        public ProductResponse(Product product)
        {
            Id = product.Id;
            Article = product.Article;
            Alias = product.Alias;
            Description = product.Description;
            Price = product.Price;
            PreviewImage = product.PreviewImage;
            IsVisible = product.IsVisible;
            Brand = product.Brand;

            Category = new ProductCategory()
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Alias = product.Category.Alias,
                Image = product.Category.Image,
                IsVisible = product.Category.IsVisible
            };
        }

        public int Id { get; set; }
        public string? Article { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string? PreviewImage { get; set; } = null!;
        public bool IsVisible { get; set; }
        public ProductCategory Category { get; set; }
        public Brand Brand { get; set; }
    }
}
