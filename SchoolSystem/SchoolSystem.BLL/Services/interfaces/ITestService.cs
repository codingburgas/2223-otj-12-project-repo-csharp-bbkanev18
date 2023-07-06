using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface ITestService
    {
        public TestAttemptTransferObject GetCurrentTest(string? testId, string? currentUserId);
        public TestAddInSectionTransferObject GetEditTest(string? testId);
        public bool UpdateTest(TestAddInSectionTransferObject transferObject);
        public QuestionInTestTransferObject GetQuestionInTest(string? testId);
        public CreateQuestionTransferObject GetCreateQuestion(string? testId);
        public CreateQuestionTransferObject GetCreateQuestion(string? testId, string? questionId);
        public bool CreateQuestion(string? testId, CreateQuestionTransferObject transferObject);
        public bool DeleteQuestion(string? testId, string? questionId);
        public bool DeleteTest(string? testId);
        public bool UpdateQuestion(string? questionId, CreateQuestionTransferObject transferObject);
        public List<AttemptTestTransferObject> GetAttemptTest(string? testId, string? currentUserId);
        public int GetMaxPoints(string? testId);
        public int GetUserPoints(string? testId, Dictionary<string, string> answers);
        public bool AddUserScore(string? testId, string? userId, int maxPoints, int userPoints);
        public ResultUserTransferObject GetResultUser(string? testId);
        public bool RemoveUserScore(string? testId, string? userId);

    }
}
