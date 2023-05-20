namespace Feipder.Entities.RequestModels
{
    public class BasketPostItemRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int SizeId { get; set; }
    }
}
