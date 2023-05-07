using Feipder.Entities;
using Feipder.Entities.Models;
using Feipder.Tools.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Feipder.Data.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public override IQueryable<Product> FindAll()
            => RepositoryContext.Set<Product>()
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Color);

        public ProductRepository(DataContext context) : base(context) { }
        public IEnumerable<Product> FindByCondition(Func<Product, bool> expression, int skipCount, int takeCount) =>
            RepositoryContext.Set<Product>()
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Color)
                .Skip(skipCount)
                .Take(takeCount)
                .Where(expression);

        public IEnumerable<Product> FindByCondition(Func<Product, bool> expression, int skipCount, int takeCount, SortMethod sortMethod)
        {
            var result = RepositoryContext.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.Color).ToList();

            var filteredCollection = result.Where(expression).Skip(skipCount).Take(takeCount).OrderBy(sortMethod).ToList();

            //.Where(expression)
            //.Skip(skipCount)
            //.Take(takeCount)
            //.OrderBy(sortMethod)
            //.ToList();
            return filteredCollection;

        }


        public override IEnumerable<Product> FindByCondition(Func<Product, bool> expression) =>
            RepositoryContext.Set<Product>().Include(x => x.Category).Include(x => x.Brand).Include(x => x.Color).Where(expression);
    }
}
