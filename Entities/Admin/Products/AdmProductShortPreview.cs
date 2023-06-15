using Feipder.Entities.ResponseModels;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Products
{
    public class AdmProductShortPreview
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Article { get; set; } = null!;

        [Required]
        public CategoryResponse Category { get; set; } = null!;
        
        [Required]
        public bool IsVisible { get; set; }
    }
}
