using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles ="user,teacher,admin")]
    public class CalendarController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        public CalendarController(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public IActionResult Index()
        {
            var model = new List<CalendarTransferObject>();
            var newEvent = new CalendarTransferObject
            {
                Name = "New Event",
                Deadline = DateTime.Now
            };
            model.Add(newEvent);
            return View(model);
        }
    }
}
