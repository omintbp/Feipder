using System;
using System.Collections.Generic;
using Feipder.Models;

namespace Feipder;

public partial class ProductSizeAvailable
{
    public int Id { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
