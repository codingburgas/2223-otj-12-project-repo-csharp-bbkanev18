using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles ="user,teacher,admin")]
    public class CalendarController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly ICalendarService _calendarService;
        public CalendarController(SchoolDBContext schoolDBContext, ICalendarService calendarService)
        {
            _schoolDBContext = schoolDBContext;
            _calendarService = calendarService;
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
