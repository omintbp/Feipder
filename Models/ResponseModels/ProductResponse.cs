namespace Feipder.Models.ResponseModels
{
    public class ProductResponse
    {
        public ProductResponse(Product product)
        {
            Id = product.Id;
            Article = product.Article;
            Alias = product.Alias;
            Description = product.Description;
            Price = product.Price;
        }

        public int Id { get; set; }
        public string Article { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
