using Feipder.Entities.Models;

namespace Feipder.Data.Repository
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public IEnumerable<Product> FindByCondition(Func<Product, bool> expression, int skipCount, int takeCount);
        public IEnumerable<Product> FindByCondition(Func<Product, bool> expression, int skipCount, int takeCount, SortMethod sortMethod);
    }
}
