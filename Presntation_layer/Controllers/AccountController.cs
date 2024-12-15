using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Presntation.Controllers;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Presntation_layer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(LoginModel login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (login.Username == "admin" && login.Password == "password")
        //        {
        //            var token = TempData["Token"] as string;
        //            var client = new HttpClient();
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //            return RedirectToAction("AllProducts", "Product"); 
        //        }
        //        else if (login.Username != "admin" && login.Password == "password")
        //        {
        //            var token = GenerateJwtToken(login.Username);
        //            TempData["Token"] = token; 
        //            return RedirectToAction("Index", "Product"); 
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid username or password");
        //        }
        //    }
        //    return View(login);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                if (login.Username == "admin" && login.Password == "password")
                {
                    // توليد التوكن لـ Admin
                    var token = GenerateJwtToken(login.Username);
                    TempData["Token"] = token;

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    return RedirectToAction("AllProducts", "Product");
                }
                else if (login.Username != "admin" && login.Password == "password")
                {
                    var token = GenerateJwtToken(login.Username);
                    TempData["Token"] = token;
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(login);
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nevergiveupuntilyougettheparadis123456789*#@$"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, username == "admin" ? "Admin" : "User")
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
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
