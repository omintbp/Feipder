using Feipder.Entities;
using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public BrandsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetBrands()
        {
            if (!_dbContext.Colors.Any())
            {
                return NotFound();
            }

            return await _dbContext.Brands.Select(x => new BrandResponse(x)).ToListAsync();
        }
    }
}
