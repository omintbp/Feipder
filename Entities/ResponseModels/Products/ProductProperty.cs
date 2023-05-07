namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public IEnumerable<ProductPropertyValue> Data { get; set;}

    }

    public class ProductPropertyValue
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;
        public int ProductsCount { get; set; } = 0;
        public string Description { get; set; } = null!;
    }
}
