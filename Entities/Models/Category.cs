﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Feipder.Entities.Models;

public partial class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The category name should not be empty")]
    [StringLength(70, ErrorMessage = "The size of the name should not exceed 70 characters")]
    public string Name { get; set; } = null!;

    [StringLength(70, ErrorMessage = "The size of the alias should not exceed 70 characters")]
    public string? Alias { get; set; }

    public int? ParentId { get; set; }
    
    public virtual Category? Parent { get; set; }

    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

    public string? Image { get; set; }

    public bool IsVisible { get; set; } = false;

    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Size> Sizes { get; set; } = new List<Size>();
}
