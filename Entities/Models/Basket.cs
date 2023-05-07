namespace Feipder.Entities.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;
}
