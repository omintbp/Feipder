using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels
{
    public class LandingResponse
    {
        [Required]
        public string RightImage { get; set; } = null!;

        [Required]
        public string LeftImage { get; set; } = null!;

        [Required]
        public string Subtitle { get; set; } = null!;

        public LandingResponse(LandingPage page)
        {
            this.Subtitle = page.Subtitle;
            this.RightImage = page.RightImage;
            this.LeftImage = page.LeftImage;
        }
    }
}
