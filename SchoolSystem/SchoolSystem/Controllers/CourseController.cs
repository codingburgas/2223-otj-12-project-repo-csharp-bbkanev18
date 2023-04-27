using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly ICourseService _courseService;
        public CourseController(SchoolDBContext schoolDBContext, ICourseService courseService)
        {
            _schoolDBContext= schoolDBContext;
            _courseService= courseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _courseService.GetCoursesUser(User?.Identity?.Name);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin, teacher")]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, teacher")]
        public IActionResult CreateCourse(CourseCreateTransferObject course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_courseService.CreateCourse(course))
            {
                ModelState.AddModelError("Name", $"Вече съществува курс с име: {course.Name}");
                return View();
            }
            TempData["Message"] = $"Курса с име: '{course.Name}' е създаден успешно.";
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles = "admin, teacher")]
        public IActionResult EditCourse(string? id)
        {
            return View(_courseService.GetCourseById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> EditCourse(Course course)
        {
            if (!ModelState.IsValid)
                return View();
            if (_courseService.EditCourse(course))
            {
                ModelState.AddModelError(string.Empty, "Има грешки в синтаксиса или това име е заето.");
                return View();
            }

            var existingCourse = await _schoolDBContext.Courses.FindAsync(course.Id);

            if (existingCourse != null)
                _courseService.DetachingCourse(existingCourse);
            _courseService.AttachCourse(course);

            _schoolDBContext.Update(course);
            _schoolDBContext.SaveChanges();

            TempData["Message"] = $"Промените по Курса с име: '{course.Name}' са успешни.";

            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles ="user,admin,teacher")]
        public IActionResult CourseSection(string? id)
        {
            var model = _courseService.GetCourseSections(id, User?.Identity?.Name ?? string.Empty);
            if(model.Count == 0)
                return Redirect("https://http.cat/401");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult AddSectionCourse(string? id) 
        {
            var model = _courseService.GetCourseSection(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="admin,teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult AddSectionCourse(CourseSectionTransferObject transferObject)
        {

            if(!ModelState.IsValid)
                return View();

            if(_courseService.CreateSectionCourse(transferObject))
            {
                var model = _courseService.GetCourseSection(transferObject.Id);
                ModelState.AddModelError(string.Empty, "Грешка при създаването на раздел!");
                return View(model);
            }

            TempData["Message"] = $"Усшесно създаден раздел с име: '{transferObject.Name}'.";

            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult EditSectionCourse(string? id)
        {
            var model = _courseService.GetSection(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult EditSectionCourse(CourseSectionTransferObject transferObject)
        {

            if (!ModelState.IsValid)
                return View();

            if (_courseService.UpdateSectionCourse(transferObject))
            {
                var model = _courseService.GetCourseSection(transferObject.Id);
                ModelState.AddModelError(string.Empty, "Грешка при създаването на раздел!");
                return View(model);
            }

            TempData["Message"] = $"Усшесна промяна на името на раздел: '{transferObject.Name}'";
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult CreateTest(string? id)
        {
            var model = _courseService.GetTestAddInSectionTransferObject(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="admin, teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTest(TestAddInSectionTransferObject transferObject)
        {
            if (!ModelState.IsValid)
            {
                var model = _courseService.GetTestAddInSectionTransferObject(transferObject.Id);
                ModelState.AddModelError(string.Empty, "Грешка в данните!");
                return View(model);
            }
            if (_courseService.CreateTest(transferObject))
            {
                var model = _courseService.GetTestAddInSectionTransferObject(transferObject.Id);
                ModelState.AddModelError(string.Empty, "Грешка при създаването на тест!");
                return View(model);
            }
            TempData["Message"] = $"Усшесно създаден тест.";
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult CreateLesson(string? id)
        {
            var model = _courseService.GetFileAddInSection(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,teache")]
        public IActionResult CreateLesson(FileAddInSectionTransferObject transferObject)
        {
            // Check if the file size is more the 5 MB
            if(transferObject?.File?.Length >= 5242880)
            {
                var model = _courseService.GetFileAddInSection(transferObject.Id);
                ModelState.AddModelError("File", "Максимално допустимият размер на качен файл е 5 MB.");
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                var model = _courseService.GetFileAddInSection(transferObject?.Id);
                ModelState.AddModelError(string.Empty, "Грешка в данните!");
                return View(model);
            }

            if (_courseService.CreateLesson(transferObject))
            {
                var model = _courseService.GetFileAddInSection(transferObject?.Id);
                ModelState.AddModelError(string.Empty, "Грешка при създаването на файл!");
                return View(model);
            }

            TempData["Message"] = $"Усшесно създаден урок.";
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles = "admin, teacher")]
        public IActionResult AddUserInCourse(string? id)
        {
            var model = _courseService.GetAddUserInCourse(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="admin, teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult AddUserInCourse(string? id, string? userId)
        {
            if(_courseService.AddUserInCourse(id ?? string.Empty, userId ?? string.Empty))
            {
                var course = _courseService.GetCourseById(id);
                var users = _schoolDBContext.Users.ToList();
                var model = new AddUserInCourseTransferObject
                {
                    Course = course ?? new Course(),
                    Users = users
                };
                ModelState.AddModelError(string.Empty, "Този потребител вече е курса!");
                return View(model);
            }
            TempData["Message"] = $"Усшесно добавен потребител в курса.";
            return RedirectToAction("UsersInCourse", new { id = id });
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult UsersInCourse(string? id)
        {
            var model = _courseService.GetSignInUsers(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveUserFromCourse(string? courseId, string? userId)
        {
            if(_courseService.DeleteUserInCourse(userId, courseId))
            {
                return Redirect("https://http.cat/409");
            }
            TempData["Message"] = $"Усшесно изтрит потребител от курса.";
            return RedirectToAction("UsersInCourse", new { id = courseId });
        }
    }
}
