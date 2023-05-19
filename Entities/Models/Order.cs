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

    public virtual Delivery Delivery { get; set; } = null!;

    public virtual DeliveryType DeliveryType { get; set; } = null!;

    public virtual OrderStatus OrderStatus { get; set; } = null!;

}
