using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public abstract class EntityImage<T> where T : class
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        [Required]
        public string Url { get; set; } = null!;

        public EntityImage() { }
    }
}
