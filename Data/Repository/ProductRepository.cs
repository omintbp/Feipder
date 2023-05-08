using Feipder.Entities;
using Feipder.Entities.Models;
using System.Linq;
using System.Linq.Expressions;
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

        public override IEnumerable<Product> FindByCondition(Func<Product, bool> expression)
        {
            var result = RepositoryContext.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Color)
                .ToList();

            return result.Where(expression);
        }
     }
}
