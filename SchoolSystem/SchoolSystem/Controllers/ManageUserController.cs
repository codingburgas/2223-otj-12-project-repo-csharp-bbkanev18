using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolSystem.BLL.Services;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles ="admin")]
    public class ManageUserController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly IManageUserService _manageUserService;
        public ManageUserController(SchoolDBContext schoolDBContext, IManageUserService manageUserService)
        {
            _schoolDBContext= schoolDBContext;
            _manageUserService= manageUserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var models = _manageUserService.GetManageUserTransferObjects();
            return View(models);
        }

        [HttpGet]
        public IActionResult EditUser(string? id)
        {
            ViewBag.Roles = _manageUserService.GetRoles();
            var model = _manageUserService.GetManageUserTransferObjectById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(ManageUserTransferObject user, string? role)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
                return View();
            if (_manageUserService.UpdateUser(user, role))
            {
                ViewBag.Roles = _manageUserService.GetRoles();
                ModelState.AddModelError(string.Empty, "There's an issue with the private data!");
                return View(user);
            }
            TempData["Message"] = $"The changes for the user with email '{user.Email}' have been successfully updated.";
            return RedirectToAction("Index", "ManageUser");
        }

        [HttpGet]
        public IActionResult DetailsUser(string? id)
        {
            var model = _manageUserService.GetManageUserTransferObjectById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string? id)
        {
            var model = _manageUserService.GetManageUserTransferObjectById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ManageUserTransferObject user)
        {
            if (ModelState["Password"]?.ValidationState != ModelValidationState.Valid
                || ModelState["ConfirmPassword"]?.ValidationState != ModelValidationState.Valid)
                return View();

            if (_manageUserService.UpdateUserPassword(user))
            {
                ModelState.AddModelError(string.Empty, "Error: It is not possible to set an old password as the new password!");
                return View(user);
            }
            TempData["Message"] = $"The password for the user with email '{user.Email}' has been changed successfully.";
            return RedirectToAction("Index", "ManageUser");
        }
    }
}
