using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels
{
    public class CategoryResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public virtual CategoryImage? Image { get; set; } = null!;

        public CategoryResponse(Category category) 
        {
            Id = category.Id;
            Name = category.Name;
            Image = category.Image;
        }
    }
}
