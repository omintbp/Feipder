﻿using Feipder.Data;
using Feipder.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts([FromQuery] string price = "0,0", [FromQuery]string Brands = "0")
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
    }
}
