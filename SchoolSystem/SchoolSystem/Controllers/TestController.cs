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
            CreateQuestionTransferObject model = _testService.GetCreateQuestion(id);
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
            CreateQuestionTransferObject model = _testService.GetCreateQuestion(id, questionId);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult EditQuestion(string? id, string? questionId)
        {
            CreateQuestionTransferObject model = _testService.GetCreateQuestion(id, questionId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult EditQuestion(string? testId,string? questionId, CreateQuestionTransferObject transferObject)
        {
            if(_testService.UpdateQuestion(questionId, transferObject))
            {
                return Redirect("https://http.cat/404");
            }
            TempData["Message"] = $"Усшесно редактиране на въпроса.";
            return RedirectToAction("Question", new { id = testId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public IActionResult DeleteTest(string? id, string? courseId)
        {
            if (_testService.DeleteTest(id))
            {
                return Redirect("https://http.cat/404");
            }
            return RedirectToAction("CourseSection", "Course", new { id = courseId });
        }

        [HttpGet]
        public IActionResult AttemptTest(string? id)
        {
            var model = _testService.GetAttemptTest(id, User?.Identity?.Name);
            if(model == null)
                return Redirect("https://http.cat/500");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinishTest(string testId, Dictionary<string, string> SelectedAnswers)
        {
            int maxPoints = _testService.GetMaxPoints(testId);
            if(maxPoints < 0)
                return Redirect("https://http.cat/404");

            int userPoints = _testService.GetUserPoints(testId, SelectedAnswers);
            if (userPoints < 0)
                return Redirect("https://http.cat/404");

            if (_testService.AddUserScore(testId, User?.Identity?.Name, maxPoints, userPoints))
                return Redirect("https://http.cat/404");
            return RedirectToAction("Index", new { id =  testId});
        }
    }
}
