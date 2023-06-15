using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Products
{
    public class AdmProductPutRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CategoryId { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }
        
        [StringLength(50)]
        public string? Alias { get; set; }

        [StringLength(50)]
        public string? Article { get; set; }

        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public bool IsVisible { get; set; }
    }
}
