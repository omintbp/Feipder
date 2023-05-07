using Feipder.Data.Repository;
using Feipder.Entities.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IList<CategoryResponse>> Get()
        {
            if (!_repository.Categories.FindAll().Any())
            {
                return NotFound();
            }

            //var result = _db.Categories.Include(x => x.Children).ToList().Where(x => x.Parent == null).ToList();
            //var treeView = new ResponseCategoryTree(result);

            return Ok();
        }

        [HttpGet("{categoryId}/sizes")]
        public ActionResult GetCategorySizes(int categoryId)
        {
            var category = _repository.Categories.FindByCondition((category) => category.Id == categoryId).FirstOrDefault();

            if (category == null)
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
