//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Presntation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        [AllowAnonymous]
//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginModel login)
//        {
//            if (login.Username == "admin" && login.Password == "password")
//            {
//                var token = GenerateJwtToken(login.Username);
//                return Ok(new { Token = token });
//            }

//            return Unauthorized();
//        }

//        private string GenerateJwtToken(string username)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nevergiveupuntilyougettheparadis123456789*#@$"));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, username),
//                new Claim(ClaimTypes.Role, "Admin")
//            };

//            var token = new JwtSecurityToken(
//                issuer: "YourIssuer",
//                audience: "YourAudience",
//                claims: claims,
//                expires: DateTime.Now.AddDays(7),
//                signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//    public class LoginModel
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous] 
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = GenerateJwtToken(login.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized(); 
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nevergiveupuntilyougettheparadis123456789*#@$"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: "YourIssuer", 
                audience: "YourAudience", 
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
