using Feipder.Data.Repository;
using Feipder.Entities.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Администрирование категорий")]
    [Authorize(Roles = "admin")]
    public class CategoriesController : ControllerBase
    {

        private readonly IRepositoryWrapper _repository;

        public CategoriesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: api/categories/
        [HttpGet]
        public ActionResult<IList<AdmCategoryNode>> Get()
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

            var treeView = new AdmCategoryTree(categories);

            return Ok(treeView.Nodes);
        }


        [HttpGet("{categoryId}")]
        public ActionResult<AdmCategoryNode> GetCategory(int categoryId)
        {
            try
            {
                var root = _repository.Categories.FindAll()
                   .Include(x => x.Image)
                   .Where(x => x.Id == categoryId)
                   .FirstOrDefault();

                if (root == null)
                {
                    return NotFound();
                }

                return Ok(new AdmCategoryNode(root));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
