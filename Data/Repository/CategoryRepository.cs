using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Feipder.Data.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext repositoryContext) : base(repositoryContext)
        {
        }

        public override IEnumerable<Category> FindByCondition(Func<Category, bool> expression)
                => RepositoryContext.Set<Category>().Include(x => x.Parent).Include(x => x.Sizes).Include(x => x.Children).Where(expression);
    }
}
