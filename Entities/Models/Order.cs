using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [StringLength(500)]
    public string? Comment { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset DateModified { get; set; }

    [Required]
    public Address Address { get; set; } = null!;

    public DeliveryType DeliveryType { get; set; }

    public OrderStatus OrderStatus { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrderItem> Items { get; set;} = new List<OrderItem>();

}
