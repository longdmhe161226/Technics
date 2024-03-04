using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        JWTDBContext _jwtDBContext = new JWTDBContext();
        public TokenController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        public IActionResult Post(User _userData)
        {

            if (_userData != null && _userData.UserName != null && _userData.PassWord != null)
            {
                var user = _jwtDBContext.Users.Include(u => u.Roles).FirstOrDefault(x => x.PassWord == _userData.PassWord && x.UserName == _userData.UserName);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserName", user.UserName)
                   };
                    foreach (var item in user.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.RoleName));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
