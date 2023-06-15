using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Colors
{
    public class PostColorRequest
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string Value { get; set; } = null!;
    }
}
