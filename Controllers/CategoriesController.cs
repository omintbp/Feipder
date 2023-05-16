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

        // GET: api/categories/
        [HttpGet]
        public ActionResult<IList<CategoryNode>> Get()
        {
            if (!_repository.Categories.FindAll().Any())
            {
                return NotFound();
            }

            var categories = _repository.Categories.FindAll()
                .Include(x => x.Parent)
                .Include(x => x.Image)
                .Include(x => x.Children)
                .ToList();

            var treeView = new CategoryTree(categories);

            return Ok(treeView.Nodes);
        }

        [HttpGet("{categoryId}")]
        public ActionResult<IList<CategoryNode>> GetCategory(int categoryId)
        {
            var root = _repository.Categories.FindAll().Include(x => x.Image).Where(x => x.Id == categoryId).FirstOrDefault();

            if (root == null)
            {
                return NotFound();
            }

            try
            {
                var categories = _repository.Categories.FindAll()
                    .Include(x => x.Parent)
                    .Include(x => x.Image)
                    .Include(x => x.Children)
                    .ToList();

                var treeView = new CategoryTree(root, categories);

                return Ok(treeView.Nodes);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("to/{categoryId}")]
        public ActionResult<IList<CategoryNode>> GetPathToCategory(int categoryId)
        {
            var category = _repository.Categories.FindByCondition((category) => category.Id == categoryId).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            var categories = _repository.Categories.FindAll()
              .Include(x => x.Parent)
              .Include(x => x.Image)
              .Include(x => x.Children)
              .ToList();

            var treeView = new CategoryTree(category, categories, false);

            return Ok(treeView.Nodes);
        }

    }
}
