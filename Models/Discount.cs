using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Discount
{
    public int Id { get; set; }

    public int Size { get; set; }

    public TimeOnly DateStart { get; set; }

    public TimeOnly DateEnd { get; set; }

    public virtual Product Product { get; set; } = null!;
}
