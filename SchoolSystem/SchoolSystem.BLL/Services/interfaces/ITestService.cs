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
        public bool CreateQuestion(string? testId, CreateQuestionTransferObject transferObject);
        public bool DeleteQuestion(string? testId, string? questionId);

    }
}
