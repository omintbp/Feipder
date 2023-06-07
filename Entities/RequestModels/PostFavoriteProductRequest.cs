using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class PostFavoriteProductRequest
    {
        [Required]
        [Range(0, Int32.MaxValue)]
        public int ProductId { get; set; }
    }
}
