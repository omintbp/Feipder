using Feipder.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext RepositoryContext { get; set; }

        public RepositoryBase(DataContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public virtual IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();

        public virtual IEnumerable<T> FindByCondition(Func<T, bool> expression) =>
            RepositoryContext.Set<T>().Where(expression);

        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}
