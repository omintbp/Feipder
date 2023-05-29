using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.PickupPoint
{
    public class PickupPointResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; } = null!;

        [StringLength(50)]
        public string? Coordinates { get; set; }

        public IEnumerable<WorkHourResponse> WorkHours { get; set; } = new List<WorkHourResponse>();
    }
}
