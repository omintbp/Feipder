using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
