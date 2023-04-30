using System;
using System.Collections.Generic;
using Feipder.Models;

namespace Feipder;

public partial class Product
{
    public int Id { get; set; }

    public string? Article { get; set; } = null!;

    public string? Alias { get; set; } = null!;

    public int Price { get; set; }

    public string? Description { get; set; } = null!;

    public int CountAvailable { get; set; }

    public string? PreviewImage { get; set; } = null!;

    public bool IsVIsible { get; set; }

    public virtual Brand? Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual Color? Color { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSizeAvailable> ProductSizeAvailables { get; set; } = new List<ProductSizeAvailable>();

    public virtual ICollection<SubscribeProduct> SubscribeProducts { get; set; } = new List<SubscribeProduct>();
}
