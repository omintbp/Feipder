using Feipder.Entities.Models;
using System.Collections;

namespace Feipder.Data.Repository
{
    public interface IColorRepository : IRepositoryBase<Color>
    {
        IEnumerable<Color> FindByProduct(Product product);
    }
}
