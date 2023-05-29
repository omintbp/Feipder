using Feipder.Entities.Models;

namespace Feipder.Entities.ResponseModels.Products
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Available { get; set; }
        public bool OnFitting { get; set; } = false;
        public bool Subscribed { get; set; } = false;

        public ProductSize()
        {

        }

        public ProductSize(Size size)
        {
            Id = size.Id;
            Value = size.Value;
            Description = size.Description;
        }
    }
}
