using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels.Products;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Orders
{
    public class OrderResponseItem
    {
        public int Id { get; set; }

        public ItemProduct Product { get; set; }

        public ProductColor Color { get; set; }

        public ProductSize Size { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Count { get; set; }
    }
}
