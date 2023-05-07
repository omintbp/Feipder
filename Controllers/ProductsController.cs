using Feipder.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feipder.Data.Repository;
using Feipder.Entities.RequestModels;
using Feipder.Data;
using Feipder.Tools.Extensions;
using Feipder.Entities.ResponseModels.Products;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.Models.ResponseModels.Products;

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
        public ActionResult<ProductPreviews> GetProducts([FromQuery]ProductsParameters queryParams, [FromQuery]SortMethod sortMethod, 
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
                var paramSizes = queryParams.Sizes.ToIntArray();

                /// получаем размеры продукта (false = получать лишь доступные размеры, т.е. размеры, у которых количество товара > 0)
                var sizes = _repository.Sizes.FindByProduct(p, false)
                    .Where(s => paramSizes.Count() == 0 || queryParams.Sizes.ToIntArray().Contains(s.Id));

                return new ProductPreview(p) { 
                    Sizes = sizes
                };

            }).ToList();

            /// минимальная цена в выборке
            var minPrice = results.Min(p => p.Price);

            /// максимальная цена в выборке
            var maxPrice = results.Max(p => p.Price);

            /// количество элементов в выборке
            var count = results.Count();

            /// используемые в выборки свойства товаров.
            /// каждое свойство содержит значение и количество товаров, которые данному значению соответствуют.
            var productsProperty = new List<ProductProperty>();

            if (withProperties)
            {
                productsProperty.Add(new ProductProperty()
                {
                    Id = 1,
                    PropertyName = nameof(Color),
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
                    PropertyName = nameof(Brand),
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

                /// собираем размеры из выборки для группировки
                var sizes = new List<ProductSize>();
                results.ForEach(result => sizes.AddRange(result.Sizes));

                productsProperty.Add(new ProductProperty()
                {
                    Id = 3,
                    PropertyName = nameof(Size),
                    Data = (from size in sizes
                            group size by new { size.Id, size.Value, size.Description} into c
                            select new ProductPropertyValue()
                            {
                                Id = c.Key.Id,
                                Value = c.Key.Value,
                                Description = c.Key.Description,
                                ProductsCount = c.Count()
                            }).ToList()
                });
            }

            return Ok(new ProductPreviews
            {
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                ProductsCount = count,
                Products = results,
                ProductProperties = productsProperty
            });
        }

        [HttpGet("{productId}")]
        public ActionResult<ProductResponse> GetProduct(int productId)
        {
            var product = _repository.Products.FindByCondition(p => p.Id == productId).FirstOrDefault();

            if(product == null)
            {
                return NotFound();
            }

            var sizes = new List<ProductSize>();

            try
            {
                sizes = _repository.Sizes.FindByProduct(product).ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            var productResponse = new ProductResponse()
            {
                Id = product.Id,
                Article = product.Article,
                Brand = product.Brand,
                Category = product.Category,
                Color = product.Color,
                Description = product.Description,
                Discount = product.Discount,
                Images = product.ProductImages,
                IsNew = product.IsNew,
                Name = product.Alias,
                Price = product.Price,
                Sizes = sizes
            };

            return Ok(productResponse);
        }
    }
}