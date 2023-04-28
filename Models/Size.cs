using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Size
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductSizeAvailable> ProductSizeAvailables { get; set; } = new List<ProductSizeAvailable>();
}
