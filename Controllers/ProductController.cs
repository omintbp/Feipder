using Feipder.Models.ResponseModels;
using Feipder.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ProductController(DataContext dbContext)
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

            var products = _dbContext.Products.Select((p) => new ProductResponse(p));

            return new JsonResult(products);
        }
    }
}
