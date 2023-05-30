using Feipder.Entities;
using Feipder.Entities.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Feipder.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Feipder.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Администрирование пользователей")]
    [Authorize(Roles = "admin")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context) 
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
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
                    FirstName = x.FirstName,
                    LastName = x.LastName,
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
        public async Task<ActionResult<AdmPanelUserResponse>> GetUser(Guid id)
        {
            try
            {
                var guests = await _userManager.GetUsersInRoleAsync("guest");
                var user = guests.FirstOrDefault(x => x.Id.Equals(id));

                if(user == null)
                {
                    return NotFound("user does not exist");
                }

                return Ok(new AdmPanelUserResponse()
                {
                    Id = id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    RegistrationDate = user.CreatedDate,
                    UpdateDate = user.UpdateDate,
                    FirstName= user.UserName,
                    LastName = user.UserName
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("admins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение информации обо всех администраторах")]
        public async Task<ActionResult<IEnumerable<AdmPanelUserResponse>>> GetAdmins()
        {
            try
            {
                /* 
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(currentUserEmail);
                */
                var adminsResult = await _userManager.GetUsersInRoleAsync("admin");

                var admins = adminsResult.Select(x => new AdmPanelUserResponse()
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    Id = Guid.Parse(x.Id),
                    LastName = x.LastName,
                    Phone = x.PhoneNumber,
                    RegistrationDate = x.CreatedDate,
                    UpdateDate = x.UpdateDate
                });
                
                return admins.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("admins/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение информации о конкретном админе")]
        public async Task<ActionResult<AdmPanelUserResponse>> GetAdmin(Guid id)
        {
            try
            {
                var usersResult = await _userManager.GetUsersInRoleAsync("admin");
                var user = usersResult.FirstOrDefault(x => x.Id.Equals(id));

                if(user == null)
                {
                    return NotFound("Такого админа нету");
                }
                
                return new AdmPanelUserResponse()
                {
                    Id = id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    RegistrationDate = user.CreatedDate,
                    UpdateDate = user.UpdateDate
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("admins/_emailCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Проверить email на уникальность", 
            Description = "Можно вызывать при заполнении поля с почтой, чтобы сразу было явно, что почта не пройдет")]
        public async Task<ActionResult> CheckEmailUnique([FromBody]UserEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                return Ok(new CheckResult() { IsValid = user == null });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("admins/_phoneCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Проверить номер телефона на уникальность", 
            Description = "Можно вызывать при заполнении поля с телефоном, чтобы сразу было явно, что телефон не пройдет")]
        public async Task<ActionResult> CheckPhoneUnique([FromBody]UserPhoneRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userManager.Users.Where(x => x.PhoneNumber.Equals(request.Phone)).FirstOrDefaultAsync();
                return Ok(new CheckResult() { IsValid = user == null });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("admins")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Добавить нового админа")]
        public async Task<ActionResult<AdmPanelUserResponse>> PostAdmin(AdmUserPost request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sameUsers = _userManager.Users
                    .Where(x => x.Email.Equals(request.Email) || x.PhoneNumber.Equals(request.Phone));

                if (sameUsers.Any())
                {
                    return BadRequest("Пользователь с такими данными уже существует");
                }

                var user = new User()
                {
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    Basket = new Basket(),
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    UpdateDate = DateTimeOffset.UtcNow,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                var creationResult = await _userManager.CreateAsync(user);

                if (!creationResult.Succeeded)
                {
                    return BadRequest(creationResult.Errors);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, "admin");

                if (!addRoleResult.Succeeded)
                {
                    return BadRequest(creationResult.Errors);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostAdmin), nameof(AccountController), new AdmPanelUserResponse()
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    RegistrationDate = user.CreatedDate,
                    UpdateDate = user.UpdateDate
                });

            }catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("admins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Обновить информацию об админе")]
        public async Task<ActionResult<AdmPanelUserResponse>> PutAdmin(AdmUserPut request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var admins = await _userManager.GetUsersInRoleAsync("admin");
                var user = admins.FirstOrDefault(x => Guid.Parse(x.Id) == request.Id);

                if(user == null)
                {
                    return BadRequest($"user with id = {request.Id} does not exist");
                }

                if((!IsPhoneUnique(request.Phone) && !user.PhoneNumber.Equals(request.Phone))
                    || (!IsEmailUnique(request.Email) && !user.Email.Equals(request.Email))){
                    return BadRequest("Измененные параметры должны быть уникальны");
                }

                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                user.PhoneNumber = request.Phone ?? user.PhoneNumber;
                user.Email = request.Email ?? user.Email;
                user.UpdateDate = DateTimeOffset.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();

                    return Ok(new AdmPanelUserResponse()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        Id = Guid.Parse(user.Id),
                        Phone = user.PhoneNumber,
                        LastName = user.LastName,
                        RegistrationDate = user.CreatedDate,
                        UpdateDate = user.UpdateDate
                    });
                }

                else
                {
                    return BadRequest(result.Errors);
                }


            }catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("admins/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Удалить админа")]
        public async Task<ActionResult<AdmPanelUserResponse>> DeleteAdmin(Guid id)
        {
            try
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(currentUserEmail);

                var userToRemove = await _userManager.FindByIdAsync(id.ToString());

                if (userToRemove == null)
                {
                    return NotFound("Пользователь не найден");
                }

                if (userToRemove.Id.Equals(currentUser.Id))
                {
                    return BadRequest("Себя удалить нельзя");
                }

                var result = await _userManager.DeleteAsync(userToRemove);

                if (!result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return BadRequest();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        private bool IsEmailUnique(string email)
            => _userManager.Users.Where(x => x.Email.Equals(email)).FirstOrDefault() == null;

        private bool IsPhoneUnique(string phone)
          => _userManager.Users.Where(x => x.PhoneNumber.Equals(phone)).FirstOrDefault() == null;
    }
}
