using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        public AuthenticationController(SchoolDBContext schoolDBContext)
        {

            _schoolDBContext = schoolDBContext;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(User user)
        {
            if (!ModelState.IsValid)
            {
                return SignIn();
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
