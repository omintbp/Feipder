using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data.Repository
{
    public class ProductStorageRepository : RepositoryBase<ProductStorage>, IProductStorageRepository
    {
        public ProductStorageRepository(DataContext repositoryContext) : base(repositoryContext)
        {
        }

       public override IQueryable<ProductStorage> FindAll() 
            => RepositoryContext.Set<ProductStorage>().Include(x => x.Product).Include(x => x.Product).AsNoTracking();
    }
}
