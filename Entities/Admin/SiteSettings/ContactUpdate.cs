using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.SiteSettings
{
    public class ContactUpdate
    {
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneWA { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        public string? Telegram { get; set; }

        public string? YouTube { get; set; }

        public string? VK { get; set; }

        public string? Instagram { get; set; }

        public string? Avito { get; set; }

        public string? Address { get; set; }

        public string? WorkTime { get; set; }

        public string? Coords { get; set; } 
    }
}
