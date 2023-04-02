using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolSystem.DAL.Models;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authentication;
using IAuthenticationService = SchoolSystem.BLL.Services.interfaces.IAuthenticationService;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SchoolSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(SchoolDBContext schoolDBContext, IAuthenticationService authenticationService)
        {
            _authenticationService= authenticationService;
            _schoolDBContext = schoolDBContext;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(UserSignInDataTransferObject user)
        {
            if (!ModelState.IsValid)
                return View();
            var claimsIdentity = _authenticationService.SignIn(user, _schoolDBContext);

            if(claimsIdentity.Claims.Count() == 0)
            {
                ModelState.AddModelError("Account Error", "Invalid Email or Password");
                return View();
            }

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true
            };

            HttpContext.SignInAsync("login", new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return RedirectToAction("SignIn", "Authentication");
        }
    }
}
