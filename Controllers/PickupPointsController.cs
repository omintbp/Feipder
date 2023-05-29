using Feipder.Entities;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.ResponseModels.PickupPoint;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;


namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickupPointsController : ControllerBase
    {
        private readonly DataContext _context;

        public PickupPointsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение информации о точках самовывоза")]
        public async Task<ActionResult<IEnumerable<PickupPointResponse>>> GetPickupPoints()
        {
            try
            {
                var points = await _context.PickupPoints
                    .Include(x => x.WorkHours)
                    .Include(x => x.Address)
                    .Select(x => new PickupPointResponse()
                    {
                        Id = x.Id,
                        Address = x.Address.ToString(),
                        Coordinates = x.Coordinates,
                        WorkHours = x.WorkHours.Select(h => new WorkHourResponse()
                        {
                            From = h.From,
                            To = h.To,
                            Day = DateTimeFormatInfo.CurrentInfo.GetDayName(h.Day)
                        })
                    }).ToListAsync();

                return Ok(points);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
