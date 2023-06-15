using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class LandingPage
    {
        public Guid Id { get; set; }

        [Required]
        public string RightImage { get; set; } = null!;

        [Required]
        public string LeftImage { get; set; } = null!;

        [Required]
        public string Subtitle { get; set; } = null!;
    }
}
