using Feipder.Data;
using Feipder.Entities.ResponseModels.Products;

namespace Feipder.Tools.Extensions
{
    public static class ProductsSortExtansion
    {
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> collection, SortMethod method) where T : ProductPreview
        {
            switch (method)
            {
                case SortMethod.ByPriceAsc: return collection.OrderBy(p => p.Price);
                case SortMethod.ByPriceDesc: return collection.OrderByDescending((p) => p.Price);
                case SortMethod.Newest: return collection.OrderBy(p => p.CreatedDate);
            }

            return collection.OrderBy(x => x.Id);
        }
    }
}
