using Feipder.Entities;
using Feipder.Entities.Admin.Colors;
using Feipder.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

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

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Изменить цвет. Испльзуется в А.Д.М.И.Н.К.Е.")]
        public async Task<ActionResult> PutColor(ColorUpdateRequest request)
        {
            try
            {
                var color = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == request.ColorId);

                if (color == null)
                {
                    return NotFound();
                }

                color.Value = request.Value ?? color.Value;
                color.Name = request.Name ?? color.Name;

                _dbContext.Colors.Update(color);
                await _dbContext.SaveChangesAsync();

                return Ok(color);

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Добавить новый цвет. Испльзуется в А.Д.М.И.Н.К.Е.")]
        public async Task<ActionResult> PostColor(PostColorRequest request)
        {
            try
            {
                var color = new Color()
                {
                    Name = request.Name,
                    Value = request.Value
                };

                var result = _dbContext.Colors.Add(color).Entity;

                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostColor), result);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{colorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Удалить цвет. Испльзуется в А.Д.М.И.Н.К.Е.")]
        public async Task<ActionResult> PutColor(int colorId)
        {
            try
            {
                var color = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == colorId);

                if (color == null)
                {
                    return NotFound();
                }

                _dbContext.Colors.Remove(color);

                await _dbContext.SaveChangesAsync();

                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
