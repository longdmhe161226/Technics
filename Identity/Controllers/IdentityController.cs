using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IdentityTestDBContext _identityTestDBContext;

        public IdentityController(UserManager<User> userManager, IdentityTestDBContext identityTestDBContext)
        {
            _userManager = userManager;
            _identityTestDBContext = identityTestDBContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_identityTestDBContext.Users);
        }

        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            return Ok(await _userManager.FindByEmailAsync(email));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            return Ok("User created successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            return Ok("User Deleted successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            return Ok("User Update successfully.");
        }

    }
}
