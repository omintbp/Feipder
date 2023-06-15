using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class Contact
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneWA { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        public string Telegram { get; set; } = null!;

        [Required]
        public string YouTube { get; set; } = null!;

        [Required]
        public string VK { get; set; } = null!;

        [Required]
        public string Instagram { get; set; } = null!;
        
        [Required]
        public string Avito { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string WorkTime { get; set; } = null!;

        [Required]
        public string Coords { get; set; } = null!;
    }
}
