using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;

namespace Feipder.Models.ResponseModels
{
    public class CategoryResponse
    {
        public CategoryResponse(Category category, int level = 0)
        {
            Id = category.Id;
            Name = category.Name;
            Alias = category.Alias;
            Image = category.Image;
            IsVisible = category.IsVisible;
            Level = level;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public string Image { get; set; }
        public IList<CategoryResponse> SubCategories { get; set; }
        public int Level { get; set; }
        public bool IsVisible { get; set; }
    }
}
