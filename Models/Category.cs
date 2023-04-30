using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Alias { get; set; }

    public int? ParentId { get; set; }
    
    public virtual Category? Parent { get; set; }

    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

    public string Image { get; set; }

    public bool IsVisible { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<Size> Sizes { get; set; } = new List<Size>();
}
