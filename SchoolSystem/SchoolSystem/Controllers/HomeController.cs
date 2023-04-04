using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using SchoolSystem.Models;
using System.Diagnostics;
using System.Security.Claims;
using IAuthenticationService = SchoolSystem.BLL.Services.interfaces.IAuthenticationService;

namespace SchoolSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public HomeController(ILogger<HomeController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            /*
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
            await HttpContext.SignOutAsync();
            */
            //IEnumerable<User> user = await _schoolDBContext.Users.ToListAsync();
            //HttpContext.SignOutAsync().Wait();

            //var userId = User?.Identity?.Name ?? string.Empty;
            //user = _authenticationService.GetUserById(userId);
            return View();
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