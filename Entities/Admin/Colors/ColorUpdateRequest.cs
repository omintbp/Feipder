using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Colors
{
    public class ColorUpdateRequest
    {
        [Required]
        [Range(0, Int32.MaxValue)]
        public int ColorId { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Value { get; set; }
    }
}
