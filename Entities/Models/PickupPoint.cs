namespace Feipder.Entities.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<OrderPickup> OrderPickups { get; set; } = new List<OrderPickup>();
}
