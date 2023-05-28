namespace Feipder.Entities.Models
{
    public class BasketItem
    {
        public int Id { get; set; }

        public int BasketId { get; set; }
        public virtual Basket Basket { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Count { get; set; }

        public int SizeId { get; set; }
        public virtual Size Size { get; set; }

        public int ColorId { get; set; }
        public virtual Color? Color { get; set; }
    }
}
