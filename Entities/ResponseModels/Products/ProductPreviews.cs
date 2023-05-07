namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductPreviews
    {
        public int ProductsCount { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public ICollection<ProductPreview> Products { get; set; } = new List<ProductPreview>();
        public ICollection<ProductProperty> ProductProperties { get; set; } = new List<ProductProperty>();
    }
}
