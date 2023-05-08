using Newtonsoft.Json;

namespace Feipder.Entities.Models;

public partial class Discount
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int Size { get; set; }

    public TimeOnly DateStart { get; set; }

    public TimeOnly DateEnd { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = null!;
}
