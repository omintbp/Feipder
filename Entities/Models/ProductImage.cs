namespace Feipder.Entities.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string Link { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
