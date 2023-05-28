using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Models
{
    public class ProductColor
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Name { get; set; } 
        
        [Required]
        [StringLength(10)]
        public string Value { get; set; }

        public ProductColor(Color color)
        {
            this.Id = color.Id;
            this.Name = color.Name;
            this.Value = color.Value;
        }
    }
}
