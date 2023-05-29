using System;
using System.Collections.Generic;

namespace Feipder.Entities.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public string Coordinates { get; set; }

}
