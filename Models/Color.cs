using Feipder.Data.Config;
using Microsoft.EntityFrameworkCore;
using DrColor = System.Drawing.Color;

namespace Feipder.Models;

[EntityTypeConfiguration(typeof(ColorConfiguration))]
public partial class Color
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

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
