using Feipder.Entities.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Products
{
    public class ProductImagePost
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string? ImageName { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile UploadedFile { get; set; } = null!;
    }
}
