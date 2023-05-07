using Feipder.Data.Config;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DrColor = System.Drawing.Color;

namespace Feipder.Entities.Models;

[EntityTypeConfiguration(typeof(ColorConfiguration))]
public partial class Color
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;

    public Color()
    {

    }

    public Color(DrColor color)
    {
        Name = color.Name;
        Value = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
