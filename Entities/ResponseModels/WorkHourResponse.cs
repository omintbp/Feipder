using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels
{
    public class WorkHourResponse
    {
        public TimeOnly From { get; set; }
        
        public TimeOnly To { get; set; }

        [Required]
        public string Day { get; set; } = null!;
    }
}
