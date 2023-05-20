namespace Feipder.Entities.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public User User { get; set; } = null!;

    public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
}
