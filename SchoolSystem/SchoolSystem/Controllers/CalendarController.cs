using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            var model = _calendarService.GetCalendarTransfers(User?.Identity?.Name ?? string.Empty);
            if(model.Count == 0)
                return Redirect("https://http.cat/404");
            return View(model);
        }
    }
}
