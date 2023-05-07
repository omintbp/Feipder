using Feipder.Entities;
using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data.Repository
{
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Size> FindByCategory(Category category)
        {
            var result = new List<Size>();

            if (category == null || category.Sizes == null)
            {
                return result;
            }

            var currentCategory = category;

            while(currentCategory!.ParentId != null && currentCategory.Sizes.Count == 0)
            {
                currentCategory = RepositoryContext.Categories.Where(p => p.Id == currentCategory.ParentId).Include(x => x.Sizes).FirstOrDefault();
            }

            result = currentCategory.Sizes.ToList();

            return result;
        }

        public IEnumerable<ProductSize> FindByProduct(Product product, bool considerEmptySizes = true)
        {
            var result = new List<ProductSize>();

            if(product == null || product.Category == null)
            {
                return result;
            }

            var sizesByProducts = RepositoryContext.Storage
                .Include(x => x.Product)
                .Include(x => x.Size)
                .Where(x => x.ProductId == product.Id)
                .Select(x => new ProductSize()
                {
                    Id = x.Size.Id,
                    Value = x.Size.Value,
                    Description = x.Size.Description,
                    Count = x.Count
                })
                .ToList();

            if (considerEmptySizes)
            {
                var test = FindByCategory(product.Category);
                
                result = (from categorySize in FindByCategory(product.Category)
                            join productSize in sizesByProducts on categorySize.Id equals productSize.Id into v_sizes
                          from size in v_sizes.DefaultIfEmpty()
                          select new ProductSize()
                            {
                                 Id = categorySize.Id,
                                 Value = categorySize.Value,
                                 Description = categorySize.Description,
                                 Count = size == null ? 0 : size.Count
                            }).ToList();
            }
            else
            {
                result = sizesByProducts;
            }
            
            return result;
        }
    }
}
