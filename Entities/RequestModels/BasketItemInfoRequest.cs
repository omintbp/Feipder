using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class BasketItemInfoRequest
    {
        [Required]
        [Range(0,Int32.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int SizeId { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int ColorId { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Count { get; set; }
    }
}
