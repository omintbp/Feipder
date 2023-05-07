using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Feipder.Entities.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Article { get; set; } = null!;

    [Required(ErrorMessage = "The product title should not be empty")]
    [StringLength(70, ErrorMessage = "The size of the title should not exceed 70 characters")]
    public string Title { get; set; } = null!;

    [StringLength(70, ErrorMessage = "The size of the alias should not exceed 70 characters")]
    public string? Alias { get; set; } = null!;

    [Required(ErrorMessage = "The product must have a specified price")]
    [Range(0, Double.MaxValue, ErrorMessage = "The price must be greater than 0")]
    public double Price { get; set; }

    public string? Description { get; set; } = null!;

    public string? PreviewImage { get; set; } = null!;

    public bool IsVisible { get; set; } = false;

    public bool IsNew { get; set; } = false;

    public virtual Brand? Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Color? Color { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Discount? Discount { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public bool ContainsIn(string filter) =>
        Category.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
                        || (Alias != null && Alias.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        || Article.Contains(filter, StringComparison.OrdinalIgnoreCase);
}
