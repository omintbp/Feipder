using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.PickupPoint
{
    public class PickupPointResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; } = null!;

        public IEnumerable<WorkHourResponse> WorkHours { get; set; } = new List<WorkHourResponse>();
    }
}
