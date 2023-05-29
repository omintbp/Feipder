using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    /// <summary>
    /// Используется для представления какого-либо адреса
    /// </summary>
    public class Address
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Index { get; set; } = null!;

        [Required]
        [StringLength(40)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Street { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string House { get; set; } = null!;

        [StringLength(10)]
        public string? Flat { get; set; } = null;

        public override string ToString() => $"{Index}, {City}, {Street} {House} {Flat}";

    }
}
