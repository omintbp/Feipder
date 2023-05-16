using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels
{
    public class BrandResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public BrandResponse(Brand brand) 
        { 
            this.Id = brand.Id;
            this.Name = brand.Name;
        }
    }
}
