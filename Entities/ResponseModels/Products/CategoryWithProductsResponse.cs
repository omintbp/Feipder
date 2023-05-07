namespace Feipder.Entities.Models.ResponseModels.Products
{
    public class CategoryWithProductsResponse
    {
        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
        public IList<ProductWithoutCategoryResponse> Products { get; set; }
    }
}
