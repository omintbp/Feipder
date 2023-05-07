using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class ProductStorage
    {
        public int Id { get; set; }
     
        public int ProductId { get; set; }
        
        public Product? Product { get; set; } = null!;
        
        public int SizeId { get; set; }
        
        public Size? Size { get; set; } = null!;

        /// <summary>
        /// Актуальное Количество товара текущего размера
        /// </summary>
        [Range(0, Int32.MaxValue, ErrorMessage = "Count must be positive")]
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Количество товара на примерке
        /// </summary>
        [Range(0, Int32.MaxValue, ErrorMessage = "Count must be positive")]
        public int OnFittingCount { get; set; } = 0;

        /// <summary>
        /// Доступное для выбора количество товара
        /// </summary>
        [Range(0, Int32.MaxValue, ErrorMessage = "Count must be positive")]
        public int AvailableCount { get; set; } = 0;
    }
}
