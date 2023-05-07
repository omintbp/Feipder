using System;
using System.Collections.Generic;
using Feipder.Entities.Models;

namespace Feipder.Entities.Models;

public partial class Order
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public TimeOnly DateCreated { get; set; }

    public TimeOnly DateModified { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual DeliveryType DeliveryType { get; set; } = null!;

    //public virtual ICollection<OrderPickup> OrderPickups { get; set; } = new List<OrderPickup>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    //public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    //public virtual User User { get; set; } = null!;
}
