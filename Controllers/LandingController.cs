using Feipder.Entities;
using Feipder.Entities.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Параметры сайта")]
    public class LandingController : ControllerBase
    {
        private readonly DataContext _context;
        
        public LandingController(DataContext context) 
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение информация о 'посадочных слайдах' (что бы это ни значило ¯\\_(ツ)_/¯)")]
        public async Task<ActionResult<LandingResponse>> Get()
        {
            try
            {
                var landing = _context.LandingPages.ToList().LastOrDefault();

                if(landing == null)
                {
                    return NotFound();
                }

                return Ok(new LandingResponse(landing));

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
