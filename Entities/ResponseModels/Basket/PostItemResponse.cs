using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Basket
{
    public class PostItemResponse
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int ProductsInBasketCount { get; set; }

        [Required]
        [Range(0,Int32.MaxValue)]
        public int BasketTotalCount { get; set; }
    }
}
