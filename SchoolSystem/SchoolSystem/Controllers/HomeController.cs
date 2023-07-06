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