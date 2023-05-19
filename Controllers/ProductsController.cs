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
using Feipder.Entities.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;

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
            [FromQuery] int limit = 20, [FromQuery] int offset = 0, [FromQuery] bool withProperties = false,
           [FromQuery]bool allPossibleProperties = false, [FromQuery]bool selectOnlyAvailableProducts = false)
        {

            /// валидация переданных в строке запроса параметров
            if (!ModelState.IsValid)
            {
                /// response - 400
                return BadRequest(ModelState);
            }

            var results = new List<ProductPreview>();

            try
            {
                /// Получаем продукты по переданным фильтрам
                results = _repository.Products.FindByCondition(p => queryParams.IsProductFits(p, _repository)).Select(p =>
                {
                    var paramSizes = queryParams.Sizes.ToIntArray();

                    /// получаем размеры продукта (allPossibleProperties = false =>
                    ///     => получать лишь доступные размеры, т.е. размеры, у которых количество товара > 0)
                    var sizes = _repository.Sizes.FindByProduct(p, allPossibleProperties)
                        .Where(s => paramSizes.Count() == 0 || queryParams.Sizes.ToIntArray().Contains(s.Id));

                    return new ProductPreview(p)
                    {
                        Sizes = sizes
                    };

                }).Where(p => p.Sizes.Any(size => (selectOnlyAvailableProducts && size.Available != 0) || !selectOnlyAvailableProducts))
                .Skip(offset)
                .Take(limit)
                .OrderBy(sortMethod)
                .ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
            if(results.Count == 0)
            {
                return NotFound();
            }

            /// минимальная цена в выборке
            var minPrice = results.Min(p => p.Price);

            /// максимальная цена в выборке
            var maxPrice = results.Max(p => p.Price);

            /// количество элементов в выборке
            var count = results.Count;

            /// используемые в выборке свойства товаров.
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
                    Data = sizes.GroupBy(x => x.Id).Select(sl => new ProductPropertyValue()
                    {
                        Value = sl.First().Value,
                        ProductsCount = sl.Count((s) => s.Available != 0),
                        Id = sl.First().Id,
                        Description = sl.First().Description
                    })
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
                return NotFound("There is no product with such id");
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
                Brand = new BrandResponse(product.Brand),
                Category = new CategoryResponse(product.Category),
                Color = product.Color,
                Description = product.Description,
                Discount = product.Discount,
                Images = product.ProductImages,
                IsNew = product.IsNew,
                Name = product.Name,
                Price = product.Price,
                Sizes = sizes
            };

            return Ok(productResponse);
        }

        [HttpGet("{productId}/sizes")]
        public ActionResult<IEnumerable<ProductSize>> GetProductSizes(int productId)
        {
            var product = _repository.Products.FindByCondition(p => p.Id == productId).FirstOrDefault();

            if (product == null)
            {
                return NotFound("There is no product with such id");
            }

            try { 
                var sizes = _repository.Sizes.FindByProduct(product).ToList();
                return Ok(sizes);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{categoryId}/recs")]
        public ActionResult<ProductPreviews> GetRecsProducts(int categoryId, int offset = 0, int limit = 20)
        {
            try
            {
                var category = _repository.Categories
                    .FindAll()
                    .Where(x => x.Id == categoryId)
                    .Include(x => x.Children)
                    .ToList();

                if (category.Count == 0) 
                {
                    return NotFound();
                }

                var categoryTree = new CategoryTree(category);

                var products = _repository.Products.FindByCondition(p => categoryTree.Contains(p.Category))
                    .ToList()
                    .Select(p => new ProductPreview(p) { Sizes = _repository.Sizes.FindByProduct(p, false) } )
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

                if (products.Count == 0)
                {
                    return NotFound();
                }

                return Ok(new ProductPreviews()
                {
                    MaxPrice = products.Max(p => p.Price),
                    MinPrice = products.Min(p => p.Price),
                    ProductsCount = products.Count,
                    Products = products,
                    ProductProperties = new List<ProductProperty>()
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}