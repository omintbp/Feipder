using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class CategoryImage
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Url { get; set; }
    }
}
