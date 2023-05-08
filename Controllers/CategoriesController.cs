using Feipder.Entities.Models.ResponseModels;
using Feipder.Entities.Models;
using Feipder.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feipder.Entities;
using Feipder.Data.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public CategoriesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public ActionResult<IList<CategoryNode>> Get()
        {
            if (!_repository.Categories.FindAll().Any())
            {
                return NotFound();
            }

            var result = _repository.Categories.FindAll()
                .Include(x => x.Parent)
                .Include(x => x.Image)
                .Include(x => x.Children)
                .ToList();

            var treeView = new CategoryTree(result);

            return Ok(treeView.Nodes);
        }

        [HttpGet("{categoryId}")]
        public ActionResult GetCategory(int categoryId)
        {
            return Ok();
        }

        private ActionResult GetCategorySizes(int categoryId)
        {
            var category = _repository.Categories.FindByCondition((category) => category.Id == categoryId).FirstOrDefault();

            if(category == null)
            {
                return NotFound();
            }

            var result = _repository.Sizes.FindByCategory(category).Select((size) => new
            {
                size.Id,
                size.Value,
                size.Description
            });

            return Ok(result);
        }
    }
}
