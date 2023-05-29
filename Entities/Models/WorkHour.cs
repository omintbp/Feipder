using Feipder.Data.Config;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    [EntityTypeConfiguration(typeof(WorkHourConfigurator))]
    public class WorkHour
    {
        public int Id { get; set; }

        /// <summary>
        /// Дата начала рабочего дня
        /// </summary>
        [Required]
        public TimeOnly From { get; set; }

        /// <summary>
        /// Дата конца рабочего дня
        /// </summary>
        [Required]
        public TimeOnly To { get; set; }
        
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;

        [JsonIgnore]
        public virtual ICollection<PickupPoint> PickupPoints { get; set; } = new List<PickupPoint>();
    }
}
