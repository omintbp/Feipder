using System;
using System.Collections.Generic;

namespace Feipder.Entities.Models;

public partial class OrderPickup
{
    public int Id { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PickupPoint PickupPoint { get; set; } = null!;
}
