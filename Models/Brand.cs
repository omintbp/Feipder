using Feipder.Data.Config;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Feipder.Models;

[EntityTypeConfiguration(typeof(BrandConfiguration))]
public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
