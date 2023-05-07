using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public ColorsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            if (!_dbContext.Colors.Any())
            {
                return NotFound();
            }

            return await _dbContext.Colors.ToListAsync();
        }
    }
}
