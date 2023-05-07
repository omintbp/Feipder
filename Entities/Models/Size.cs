using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models;

public partial class Size
{
    public int Id { get; set; }

    [Required(ErrorMessage = "size value must be set")]
    public string Value { get; set; } = null!;

    public string? Description { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

}
