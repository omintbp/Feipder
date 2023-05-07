using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels;

namespace Feipder.Data.Repository
{
    public interface ISizeRepository : IRepositoryBase<Size>
    {
        IEnumerable<Size> FindByCategory(Category category);
        IEnumerable<ProductSize> FindByProduct(Product p, bool considerEmptySizes = true);
    }
}
