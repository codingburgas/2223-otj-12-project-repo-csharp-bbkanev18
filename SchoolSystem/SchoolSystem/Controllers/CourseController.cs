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
        public async Task<IActionResult> Index()
        {
            var courses = await _schoolDBContext.Courses.ToListAsync();
            return View(courses);
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
                ModelState.AddModelError("Name", $"There are alraedy course name: {course.Name}");
                return View();
            }

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
                ModelState.AddModelError(string.Empty, "There are error in syntax or this name is taken");
                return View();
            }

            var existingCourse = await _schoolDBContext.Courses.FindAsync(course.Id);

            if (existingCourse != null)
                _courseService.DetachingCourse(existingCourse);
            _courseService.AttachCourse(course);

            _schoolDBContext.Update(course);
            _schoolDBContext.SaveChanges();

            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        [Authorize(Roles ="user,admin,teacher")]
        public IActionResult CourseSection(string? id)
        {
            var model = _courseService.GetCourseSections(id);
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
                ModelState.AddModelError(string.Empty, "Error in creating section!");
                return View(model);
            }

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
                ModelState.AddModelError(string.Empty, "Error in creating section!");
                return View(model);
            }

            return RedirectToAction("Index", "Course");
        }
    }
}
