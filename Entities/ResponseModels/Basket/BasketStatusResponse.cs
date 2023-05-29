using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Basket
{
    public class BasketStatusResponse
    {
        [Required]
        [Range(0, Int32.MaxValue)]
        public int ProductsCount { get; set; }
        
        [Required]
        [Range(0, Double.MaxValue)]
        public double TotalPrice { get; set; }
        
        [Required]
        [Range(0,Double.MaxValue)]
        public double PriceWithDiscount { get; set; }
    }
}
