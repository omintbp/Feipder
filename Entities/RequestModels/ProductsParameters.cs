using AutoFixture;
using Feipder.Data.Repository;
using Feipder.Entities.Attributes;
using Feipder.Entities.Models;
using Feipder.Entities.Models.ResponseModels;
using Feipder.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;

        public bool Discount { get; set; } = false;
        public bool NewProducts { get; set; } = false;

        public bool InSubCategories { get; set; } = true;

        public string Filter { get; set; } = "";

        private bool IsPriceValid(Product product) => MinPrice <= product.Price && MaxPrice >= product.Price;
        private bool IsBrandsValid(Product product) => Brands == null || Brands.ContainsId(product.Brand.Id);
        private bool IsColorsValid(Product product, DataContext context)
        {
            if(Colors == null)
            {
                return true;
            }

            try
            {
                // rm repository
                var productColors = context.Colors.Include(x => x.Products).Where(x => x.Products.Contains(product)).Select(x => x.Id).ToList();

                var colors = Colors.ToIntArray().ToList();

                return colors.Intersect(productColors).Count() != 0;

            }
            catch (Exception)
            {

            }

            return false;

        }
            //Colors == null || Colors.ContainsId(product.Color.Id);
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

        private bool IsCategoriesValid(Product product, IRepositoryWrapper repository) 
        {
            if(Categories == null)
            {
                return true;
            }

            if (InSubCategories)
            {
                var categoriesIds = Categories.ToIntArray().ToList();

                foreach(var id in categoriesIds)
                {
                    var loadedCategories = repository.Categories.FindAll().Include(x => x.Children).ToList();
                    var root = loadedCategories.Where(x => x.Id == id).FirstOrDefault();

                    if(root == null)
                    {
                        return false;
                    }

                    var categoryTree = new CategoryTree(root, loadedCategories);

                    if (categoryTree.Contains(product.Category))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return Categories.ContainsId(product.Category.Id);
            }
        }

        private bool IsNewProductValid(Product product) => NewProducts ? product.IsNew == NewProducts : true;
        private bool IsFilterValid(Product product) => product.ContainsIn(Filter);

        public bool IsProductFits(Product p, IRepositoryWrapper rep, DataContext context)
        {
            try
            {
                var result = IsPriceValid(p) &&
                    IsBrandsValid(p) &&
                    IsSizesValid(p, rep) &&
                    IsColorsValid(p, context) &&
                    IsCategoriesValid(p, rep) &&
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
