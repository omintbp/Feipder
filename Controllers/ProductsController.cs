using Feipder.Data;
using Feipder.Data.Repository;
using Feipder.Entities.Models;
using Feipder.Entities.RequestModels;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.ResponseModels.Products;
using Feipder.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public ProductsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult GetProducts([FromQuery] ProductsParameters queryParams, [FromQuery] SortMethod sortMethod,
            [FromQuery] int limit = 20, [FromQuery] int offset = 0, [FromQuery] bool withProperties = false)
        {

            /// валидация переданных в строке запроса параметров
            if (!ModelState.IsValid)
            {
                /// response - 400
                return BadRequest(ModelState);
            }

            /// Получаем продукты по переданным фильтрам
            var results = _repository.Products.FindByCondition(p => queryParams.IsProductFits(p, _repository), offset, limit, sortMethod).Select(p =>
            {
                /// получаем размеры продукта (false = получать лишь доступные размеры, т.е. размеры, у которых количество товара > 0)
                var sizes = _repository.Sizes.FindByProduct(p, false).Where(s => queryParams.Sizes.ToIntArray().Contains(s.Id));

                return new ProductPreview(p)
                {
                    Sizes = sizes
                };

            }).ToList();

            /// минимальная цена в выборке
            var minPrice = results.Min(p => p.Price);

            /// максимальная цена в выборке
            var maxPrice = results.Max(p => p.Price);

            /// количество элементов в выборке
            var count = results.Count();


            var productsProperty = new List<ProductProperty>();

            if (withProperties)
            {
                productsProperty.Add(new ProductProperty()
                {
                    Id = 1,
                    PropertyName = nameof(Product.Color),
                    Data = (from result in results
                            group result by new { result.Color!.Id, result.Color.Value, result.Color.Name } into c
                            select new ProductPropertyValue()
                            {
                                Id = c.Key.Id,
                                Value = c.Key.Value,
                                Description = c.Key.Name,
                                ProductsCount = c.Count()
                            }).ToList()
                });

                productsProperty.Add(new ProductProperty()
                {
                    Id = 2,
                    PropertyName = nameof(Product.Brand),
                    Data = (from result in results
                            group result by new { result.Brand!.Id, result.Brand.Name } into c
                            select new ProductPropertyValue()
                            {
                                Id = c.Key.Id,
                                Value = c.Key.Name,
                                Description = null,
                                ProductsCount = c.Count()
                            }).ToList()
                });

                var sizes = new List<ProductSize>();

                results.ForEach(result => sizes.AddRange(result.Sizes));

                productsProperty.Add(new ProductProperty()
                {
                    Id = 3,
                    PropertyName = nameof(Size),
                    Data = (from size in sizes
                            group size by new { size.Id, size.Value, size.Description } into c
                            select new ProductPropertyValue()
                            {
                                Id = c.Key.Id,
                                Value = c.Key.Value,
                                Description = c.Key.Description,
                                ProductsCount = c.Count()
                            }).ToList()
                });
            }
            return new JsonResult(new
            {
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                ProductsCunt = count,
                Products = results,
                ProducsProperty = productsProperty
            });
        }
    }
}