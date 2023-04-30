using Feipder.Data;
using Feipder.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ProductsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// GET: api/products
        /// </summary>
        /// <returns>all products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            if(_dbContext.Products == null)
            {
                return NotFound();
            }

            var products = _dbContext.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Select((p) => new ProductResponse(p));

            return Ok(products);
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(int id)
        {
            if (!_dbContext.Products.Any() || !_dbContext.Categories.Any())
            {
                return NotFound();
            }

            var category = await _dbContext.Categories.FindAsync(id);
                
            if(category == null)
            {
                return NotFound();
            }

            var products = await _dbContext.Products
                .Include(x => x.Category)
                .Where(x => x.Category.Id == id)
                .Include(x => x.Brand)
                .Select(x => 
                    new {
                        Id = x.Id,
                        Article = x.Article,
                        Description = x.Description,
                        Price = x.Price,
                        PreviewImage = x.PreviewImage,
                        Alias = x.Alias,
                        Brand = x.Brand,
                        IsVisible = x.IsVIsible
                    }
                ).ToListAsync();

            return Ok(new {
                Id = category.Id,
                Category = category.Name,
                Products = products
            });
        }
    }
}
