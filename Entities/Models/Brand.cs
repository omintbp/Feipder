using Feipder.Data.Config;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Feipder.Entities.Models;

[EntityTypeConfiguration(typeof(BrandConfiguration))]
public partial class Brand
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The brand name must be set")]
    [StringLength(70, ErrorMessage = "The size of the name should not exceed 70 characters")]
    public string Name { get; set; } = null!;

    [JsonIgnore]
    public string Logo { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
