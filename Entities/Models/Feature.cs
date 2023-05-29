using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
