using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class BasketPostItemRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Count { get; set; }

        [Required]
        public int SizeId { get; set; }

        [Required]
        public int ColorId { get; set; }
    }
}
