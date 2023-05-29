using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models;

/// <summary>
/// Точка вывоза товара.
/// </summary>
public partial class PickupPoint
{
    public int Id { get; set; }

    /// <summary>
    /// Адрес точки вывоза
    /// </summary>
    [Required]
    public Address Address { get; set; } = null!;

    /// <summary>
    /// Координаты точки вывоза для показа на карте
    /// </summary>
    [StringLength(100)]
    public string? Coordinates { get; set; }

    /// <summary>
    /// Часы работы по дням
    /// </summary>
    public virtual ICollection<WorkHour> WorkHours { get; set; } = new List<WorkHour>();

}
