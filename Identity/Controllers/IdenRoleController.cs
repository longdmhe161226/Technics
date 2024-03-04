using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdenRoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IdentityTestDBContext _identityTestDBContext;

        public IdenRoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IdentityTestDBContext identityTestDBContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _identityTestDBContext = identityTestDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_identityTestDBContext.Roles);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    return Ok($"Role {roleName} created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create role.");
                }
            }
            else
            {
                return BadRequest($"Role {roleName} already exists.");
            }
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok($"Role {roleName} deleted successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete role.");
                }
            }
            else
            {
                return BadRequest($"Role {roleName} does not exist.");
            }
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User {user.UserName} added to role {roleName} successfully.");
            }
            else
            {
                return BadRequest("Failed to add user to role.");
            }
        }

        [HttpGet("IsUserInRole")]
        public async Task<IActionResult> IsUserInRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            return Ok(isInRole);
        }
    }
}
