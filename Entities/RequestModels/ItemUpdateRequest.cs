using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.RequestModels
{
    public class ItemUpdateRequest
    {
        /// <summary>
        /// Id товара, количество которого нужно обновить
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        
        /// <summary>
        /// Id размера товара, количество которого нужно обновить
        /// </summary>
        [Required]
        public int SizeId { get; set; }

        /// <summary>
        /// Id цвета товара, количество которого нужно обновить
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// Новое количество товара
        /// </summary>
        [Required]
        [Range(0, Int32.MaxValue)]
        public int NewCount { get; set; }
    }
}
