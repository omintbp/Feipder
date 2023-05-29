using Feipder.Entities;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.ResponseModels.PickupPoint;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<PickupPointResponse>>> GetPickupPoints()
        {
            try
            {
                var points = _context.PickupPoints
                    .Include(x => x.WorkHours)
                    .Include(x => x.Address)
                    .Select(x => new PickupPointResponse()
                    {
                        Id = x.Id,
                        Address = x.Address.ToString(),
                        WorkHours = x.WorkHours.Select(h => new WorkHourResponse()
                        {
                            From = h.From,
                            To = h.To,
                            Day = DateTimeFormatInfo.CurrentInfo.GetDayName(h.Day)
                        })
                    });

                return Ok(points);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
