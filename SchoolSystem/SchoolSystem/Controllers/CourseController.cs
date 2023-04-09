using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        public CourseController(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext= schoolDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _schoolDBContext.Courses.ToListAsync();
            return View(courses);
        }

        [HttpPost]
        public IActionResult AddCourse()
        {
            return View();
        }


        public IActionResult CourseSection()
        {
            return View();
        }
    }
}
