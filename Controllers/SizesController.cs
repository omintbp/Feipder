using Feipder.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public SizesController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult GetSizes()
        {
            return Ok(_repository.Sizes.FindAll());
        }
    }
}
