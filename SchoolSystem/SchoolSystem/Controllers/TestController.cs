using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles ="admin,user,teacher")]
    public class TestController : Controller
    {
        private readonly SchoolDBContext _schoolDBContext;
        private readonly ITestService _testService;

        public TestController(SchoolDBContext schoolDBContext, ITestService testService)
        {
            _schoolDBContext = schoolDBContext;
            _testService = testService;
        }

        public IActionResult Index(string? id)
        {
            var model = _testService.GetCurrentTest(id, User?.Identity?.Name);
            return View(model);
        }
    }
}
