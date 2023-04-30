using Feipder.Data;
using Feipder.Models;
using Feipder.Models.ResponseModels;
using Feipder.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _db;

        public CategoriesController(DataContext context)
        {
            _db = context;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            if (!_db.Categories.Any())
            {
                return NotFound();
            }
            
            var result = _db.Categories.Include(x => x.Children).ToList().Where(x => x.Parent == null).ToList();
            var treeView = new ResponseCategoryTree(result);

            return Ok(treeView.Nodes);
        }
    }
}
