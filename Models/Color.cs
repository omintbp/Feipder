using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Color
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
