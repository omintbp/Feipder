using Feipder.Entities;
using Feipder.Entities.Admin.SiteSettings;
using Feipder.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Параметры сайта")]
    public class ContactsController : ControllerBase
    {
        private readonly DataContext _context;

        public ContactsController(DataContext context) 
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение контактной информации для футера, как полагаю")]
        public async Task<ActionResult<Contact>> Get()
        {
            try
            {
                var contacts = _context.Contacts.ToList().FirstOrDefault();

                return Ok(contacts);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Изменить contactus notitia. Испльзуется в А.Д.М.И.Н.К.Е.")]
        public async Task<ActionResult> Put(ContactUpdate request)
        {
            try
            {
                var contactInfo = _context.Contacts.ToList().FirstOrDefault();

                if(contactInfo == null)
                {
                    return BadRequest("фигня какая-то, давай заново");
                }

                contactInfo.YouTube = request.YouTube ?? contactInfo.YouTube;
                contactInfo.VK = request.VK ?? contactInfo.VK;
                contactInfo.Coords = request.Coords ?? contactInfo.Coords;
                contactInfo.Avito = request.Avito ?? contactInfo.Avito;
                contactInfo.Address = request.Address ?? contactInfo.Address;
                contactInfo.Email = request.Email ?? contactInfo.Email;
                contactInfo.Instagram = request.Instagram?? contactInfo.Instagram;
                contactInfo.Phone = request.Phone ?? contactInfo.Phone;
                contactInfo.PhoneWA = request.PhoneWA ?? contactInfo.PhoneWA;
                contactInfo.Telegram = request.Telegram ?? contactInfo.Telegram;

                _context.Contacts.Update(contactInfo);

                await _context.SaveChangesAsync();

                return Ok(contactInfo);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
