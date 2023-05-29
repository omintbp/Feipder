using Feipder.Entities;
using Feipder.Entities.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Feipder.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Feipder.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Администрирование пользователей")]
    [Authorize(Roles = "admin")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение всех пользователей (кроме администраторов)")]
        public async Task<ActionResult<AdmPanelUsersResponse>> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("guest");

                var result = users.ToList().Select(x => new AdmPanelUserResponse()
                {
                    Id = Guid.Parse(x.Id),
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    UserName = x.FirstName,
                    UpdateDate = x.UpdateDate,
                    RegistrationDate = x.CreatedDate
                });

                return Ok(new AdmPanelUsersResponse()
                {
                    Users = result
                });

            }catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("users/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение информации о конкретном пользоветеле (кроме админа)")]
        public async Task<ActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if(user == null)
                {
                    return NotFound("user does not exist");
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("admin"))
                {
                    return NotFound();
                }

                return Ok(new AdmPanelUserResponse()
                {
                    Id = id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    RegistrationDate = user.CreatedDate,
                    UpdateDate = user.UpdateDate,
                    UserName = user.UserName
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
