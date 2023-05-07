using System.Linq.Expressions;

namespace Feipder.Data.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IEnumerable<T> FindByCondition(Func<T, bool> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
