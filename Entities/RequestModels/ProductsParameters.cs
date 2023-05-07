using AutoFixture;
using Feipder.Data.Repository;
using Feipder.Entities.Attributes;
using Feipder.Entities.Models;
using Feipder.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Feipder.Entities.RequestModels
{
    [BindProperties]
    public class ProductsParameters
    {
        [StringListParameter(ErrorMessage = "It should be a comma-separated list of brands [id1,id2,id3] (Without [])")]
        public string? Brands { get; set; } = null!;
        [StringListParameter(ErrorMessage = "It should be a comma-separated list of sizes [id1,id2,id3] (Without [])")]
        public string? Sizes { get; set; } = null!;
        [StringListParameter(ErrorMessage = "It should be a comma-separated list of colors [id1,id2,id3] (Without [])")]
        public string? Colors { get; set; } = null!;
        [StringListParameter(ErrorMessage = "It should be a comma-separated list of categories [id1,id2,id3] (Without [])")]
        public string? Categories { get; set; } = null!;

        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = Double.MaxValue;

        public bool Discount { get; set; } = false;
        public bool NewProducts { get; set; } = false;

        public string Filter { get; set; } = "";

        private bool IsPriceValid(Product product) => product.Price.InRange(MinPrice, MaxPrice);

        private bool IsBrandsValid(Product product) => Brands == null || Brands.ContainsId(product.Brand.Id);
        private bool IsColorsValid(Product product) => Colors == null || Colors.ContainsId(product.Color.Id);
        private bool IsSizesValid(Product product, IRepositoryWrapper repository)
        {
            if(Sizes == null)
            {
                return true;
            }

            var productSizes = repository.Sizes.FindByProduct(product, false).Select(p => p.Id).ToList();
            try
            {
                var querySizes = Sizes.Split(',').Select(id => Convert.ToInt32(id)).ToList();

                var intersect = querySizes.Intersect(productSizes).ToList();

                return intersect.Count() != 0;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool IsCategoriesValid(Product product) => Categories == null || Categories.ContainsId(product.Category.Id);
        private bool IsNewProductValid(Product product) => product.IsNew == NewProducts;
        private bool IsFilterValid(Product product) => product.ContainsIn(Filter);
        public ProductsParameters()
        {

        }


        public bool IsProductFits(Product p, IRepositoryWrapper rep)
        {
            try
            {
                var result = IsPriceValid(p) &&
                    IsBrandsValid(p) &&
                    IsColorsValid(p) &&
                    IsSizesValid(p, rep) &&
                    IsCategoriesValid(p) &&
                    IsNewProductValid(p) &&
                    IsFilterValid(p);

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }
    }
}
