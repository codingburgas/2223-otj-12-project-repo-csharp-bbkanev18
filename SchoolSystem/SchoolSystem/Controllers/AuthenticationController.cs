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
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService= authenticationService;
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

            var claimsIdentity = _authenticationService.SignIn(user);

            if(claimsIdentity.Claims.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
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
        public IActionResult SignUp(UserSignUpDataTransferObject user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_authenticationService.SignUp(user))
            {
                ModelState.AddModelError("Email", "This email already exist");
                return View();
            }

            return RedirectToAction("SignIn", "Authentication");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
