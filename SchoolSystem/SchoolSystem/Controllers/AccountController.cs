using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles ="admin,teacher,user,guest")]
    public class AccountController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly IAccountService _accountService;
        public AccountController(SchoolDBContext schoolDBContext, IAccountService accountService)
        {
            _schoolDBContext= schoolDBContext;
            _accountService= accountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentUser = _accountService.GetUserById(User?.Identity?.Name ?? string.Empty);
            if (currentUser == null)
                return NotFound();

            return View(currentUser);
        }

        [HttpGet]
        public IActionResult EditAccount(string? id)
        {

            var currentUser = _accountService.GetUserById(User?.Identity?.Name ?? string.Empty);
            if (currentUser == null)
                return NotFound();
            if(currentUser.Id != id)
                return Redirect("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(UserSignUpDataTransferObject newUser)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
                return View();
            if(_accountService.UpdateUser(newUser))
            {
                ModelState.AddModelError(string.Empty, "Error in saving data!");
                return View();
            }
            TempData["Message"] = "Your changes have been saved.";
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public IActionResult ChangePassword(string? id)
        {

            var currentUser = _accountService.GetUserById(User?.Identity?.Name ?? string.Empty);
            if (currentUser == null)
                return NotFound();
            if (currentUser.Id != id)
                return Redirect("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(UserSignUpDataTransferObject newUser)
        {
            if (ModelState["Password"]?.ValidationState != ModelValidationState.Valid
                || ModelState["ConfirmPassword"]?.ValidationState != ModelValidationState.Valid)
                return View();

            if (_accountService.UpdateUserPassword(newUser))
            {
                ModelState.AddModelError(string.Empty, "Error: It is not possible to set your old password as the new password!");
                return View();
            }
            TempData["Message"] = "Your password has been changed successfully.";
            return RedirectToAction("Index", "Account");
        }
    }
}
