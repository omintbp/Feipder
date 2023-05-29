using Feipder.Entities;
using Feipder.Entities.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Feipder.Entities.Models;

namespace Feipder.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AdmPanelUsersResponse>> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("guest");

                var result = users.ToList().Select(x => new AdmPanelUserResponse()
                {
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    UserName = x.FirstName
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
    }
}
