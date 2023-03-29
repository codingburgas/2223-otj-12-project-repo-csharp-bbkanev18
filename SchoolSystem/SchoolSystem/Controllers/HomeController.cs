using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.DAL.Models;
using SchoolSystem.Models;
using System.Diagnostics;

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