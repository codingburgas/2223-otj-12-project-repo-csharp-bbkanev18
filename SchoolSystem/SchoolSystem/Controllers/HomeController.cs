using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.DAL.Models;
using SchoolSystem.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace SchoolSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolDBContext _schoolDBContext;

        public HomeController(ILogger<HomeController> logger, SchoolDBContext schoolDBContext)
        {

            _schoolDBContext = schoolDBContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user1 = _schoolDBContext.Users.ToArray();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user1[0].FirstName),
                new Claim(ClaimTypes.Role, "user")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "login");

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true
            };

            HttpContext.SignInAsync("login", new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            IEnumerable<User> user = await _schoolDBContext.Users.ToListAsync();
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}