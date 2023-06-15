using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.Admin.Products
{
    public class AdmProductsList
    {
        [Range(0, Int32.MaxValue)]
        public int Count { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Offset { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Limit { get; set; }

        public IEnumerable<AdmProductShortPreview> Products { get; set; } = new List<AdmProductShortPreview>();
    }
}
