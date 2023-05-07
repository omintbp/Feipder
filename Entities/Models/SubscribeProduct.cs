using System;
using System.Collections.Generic;

namespace Feipder.Entities.Models;

public partial class SubscribeProduct
{
    public int Id { get; set; }

    public virtual Product Product { get; set; } = null!;

   // public virtual User User { get; set; } = null!;
}
