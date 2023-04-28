using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public bool IsVisible { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category Parent { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();
}
