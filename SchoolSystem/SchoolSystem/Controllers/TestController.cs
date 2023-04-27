using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
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

        [HttpGet]
        public IActionResult Index(string? id)
        {
            var model = _testService.GetCurrentTest(id, User?.Identity?.Name);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult EditTest(string? id)
        {
            var model = _testService.GetEditTest(id);
            return View(model);
            //return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles ="admin,teacher")]
        public IActionResult EditTest(TestAddInSectionTransferObject transferObject)
        {
            if (_testService.UpdateTest(transferObject))
            {
                var model = _testService.GetEditTest(transferObject.Id);
                ModelState.AddModelError(string.Empty, "Грешка в данните!");
                return View(model);
            }
            TempData["Message"] = $"Усшесно редактиране на теста.";
            return RedirectToAction("Index", new { id = transferObject.Id });
        }


        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult Question(string? id)
        {
            var model = _testService.GetQuestionInTest(id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult AddQuestion(string? id)
        {
            var model = _testService.GetCreateQuestion(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult AddQuestion(string? testId,CreateQuestionTransferObject transferObject)
        {
            if(_testService.CreateQuestion(testId, transferObject))
            {
                var model = _testService.GetCreateQuestion(testId);
                ModelState.AddModelError(string.Empty, "Грешка в данните!");
                return View(model);
            }
            TempData["Message"] = $"Усшесно добавен въпрос.";
            return RedirectToAction("Question", new { id = testId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult RemoveQuestion(string? id, string? questionId)
        {
            if(_testService.DeleteQuestion(id, questionId))
            {
                return Redirect("https://http.cat/404");
            }
            TempData["Message"] = $"Усшесно премахнат въпрос.";
            return RedirectToAction("Question", new { id = id });
        }

        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult DetailsQuestion(string? id, string? questionId)
        {
            var model = _testService.GetCreateQuestion(id, questionId);
            return View(model);
        }
    }
}
