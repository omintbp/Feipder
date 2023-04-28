using System;
using System.Collections.Generic;
using Feipder.Models;

namespace Feipder;

public partial class FavoriteProduct
{
    public int Id { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
