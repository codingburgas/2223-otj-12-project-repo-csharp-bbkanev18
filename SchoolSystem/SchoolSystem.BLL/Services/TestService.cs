using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services
{
    public class TestService : ITestService
    {
        private readonly SchoolDBContext _schoolDBContext;
        public TestService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public TestAttemptTransferObject GetCurrentTest(string? testId, string? currentUserId)
        {
            var test = _schoolDBContext.Tests.Include(cs => cs.CourseSections)
                .Include(q => q.Questions)
                .Where(t => t.Id == testId).FirstOrDefault();
            var currentUser = _schoolDBContext.Users.Include(ut => ut.UsersTests)
                .Where(cu => cu.Id == currentUserId).FirstOrDefault();
            var courseId = string.Empty;
            var isCurrentUserMakeTest = false;
            var userScore = 0;

            if (test?.CourseSections != null)
            {
                foreach (var course in test.CourseSections)
                {
                    courseId = course.CourseId;
                    break;
                }
            }

            if (currentUser?.UsersTests != null)
            {
                foreach (var testScore in currentUser.UsersTests)
                {
                    if (testScore.TestId == test?.Id)
                    {
                        isCurrentUserMakeTest = true;
                        userScore = testScore.Score;
                        break;
                    }
                }
            }

            return new TestAttemptTransferObject
            {
                Id = test?.Id ?? string.Empty,
                TestName = test?.Name ?? string.Empty,
                Deadline = test?.Deadline,
                TimeLimit = test?.TimeLimit ?? 1,
                IsCurrentUserMakeTest = isCurrentUserMakeTest,
                UserScore = userScore,
                CourseId = courseId,
                QuestionInTest = test?.Questions.Count ?? 0
            };

        }

        public TestAddInSectionTransferObject GetEditTest(string? testId)
        {
            var test = _schoolDBContext.Tests
                .Include(cs => cs.CourseSections)
                .Where(tests => tests.Id == testId)
                .First();
            var courseSection = new CoursesSection();
            foreach (var course in test.CourseSections)
            {
                courseSection.Id = course.Id;
                courseSection.Name = course.Name;
                courseSection.CourseId = course.CourseId;
                break;
            }


            return new TestAddInSectionTransferObject
            {
                Id = test?.Id ?? string.Empty,
                Name = test?.Name ?? string.Empty,
                TimeLimit = test?.TimeLimit ?? 1,
                Deadline = test?.Deadline,
                CourseId = courseSection.CourseId,
                SectionName = courseSection.Name,
                SectionId = courseSection.Id,
            };
        }

        public bool UpdateTest(TestAddInSectionTransferObject transferObject)
        {
            var currentTest = _schoolDBContext.Tests.Find(transferObject.Id);
            if (currentTest == null)
                return true;

            currentTest.Name = transferObject.Name;
            currentTest.TimeLimit = transferObject.TimeLimit;
            currentTest.Deadline = transferObject.Deadline;

            _schoolDBContext.SaveChanges();

            return false;

        }

        public QuestionInTestTransferObject GetQuestionInTest(string? testId)
        {
            var currentTest = _schoolDBContext.Tests
                .Include(q => q.Questions)
                .Where(tests => tests.Id == testId)
                .First();
            var questions = currentTest.Questions.ToList();
            return new QuestionInTestTransferObject
            {
                Test = currentTest,
                Questions = questions
            };
        }

        public CreateQuestionTransferObject GetCreateQuestion(string? testId)
        {
            var currentTest = _schoolDBContext.Tests.Find(testId);
            return new CreateQuestionTransferObject
            {
                Test = currentTest ?? new Test()
            };
        }

        public bool CreateQuestion(string? testId, CreateQuestionTransferObject transferObject)
        {
            var currentTest = _schoolDBContext.Tests.Find(testId);
            if (transferObject == null || currentTest == null)
                return true;

            var question = new Question();

            question.Name = transferObject.QuestionName;
            question.Points = transferObject.Points;
            question.Tests.Add(currentTest);

            _schoolDBContext.Add(question);
            _schoolDBContext.SaveChanges();

            var correctAnswer = CreateAnswer(transferObject.CorrectAnswer);

            ConnectQuestionAnswer(question.Id, correctAnswer.Id, true);

            foreach (var answers in transferObject.Answers ?? new List<string>())
            {
                if (answers == null)
                    break;
                if (answers != null)
                {
                    var asnwer = CreateAnswer(answers);
                    ConnectQuestionAnswer(question.Id, asnwer.Id, false);
                }

            }
            return false;
        }

        private Answer CreateAnswer(string answer)
        {
            var temp = new Answer();
            temp.Name = answer;
            _schoolDBContext.Add(temp);
            _schoolDBContext.SaveChanges();
            return temp;
        }

        private void ConnectQuestionAnswer(string questionId, string answerId, bool isCorrect)
        {
            var questionAnswer = new QuestionsAnswer();
            questionAnswer.QuestionId = questionId;
            questionAnswer.AnswerId = answerId;
            questionAnswer.IsCorrect = isCorrect;
            _schoolDBContext.Add(questionAnswer);
            _schoolDBContext.SaveChanges();
        }

        public bool DeleteQuestion(string? testId, string? questionId)
        {
            var question = _schoolDBContext.Questions
                 .Include(qa => qa.QuestionsAnswers)
                 .Include(t => t.Tests)
                 .Where(questions => questions.Id == questionId)
                 .First();

            var currentTest = _schoolDBContext.Tests.Find(testId);

            if (question == null || currentTest == null)
                return true;

            question.Tests.Remove(currentTest);

            var answers = new List<string>();
            foreach (var questionAnswer in question.QuestionsAnswers)
            {
                answers.Add(questionAnswer.AnswerId.ToString());
                _schoolDBContext.Remove(questionAnswer);
            }

            foreach (var answer in answers)
            {
                if (DeleteAnswer(answer))
                    return true;
            }


            _schoolDBContext.Remove(question);
            _schoolDBContext.SaveChanges();

            return false;

        }

        private bool DeleteAnswer(string? answerId)
        {
            var answer = _schoolDBContext.Answers.Find(answerId);
            if (answer == null)
                return true;
            _schoolDBContext.Remove(answer);
            return false;
        }

        private string GetAnswerName(string? answerId)
        {
            var answer = _schoolDBContext.Answers.Find(answerId);
            if (answer == null)
                return string.Empty;
            return answer.Name;
        }

        public CreateQuestionTransferObject GetCreateQuestion(string? testId, string? questionId)
        {
            var currentTest = _schoolDBContext.Tests.Find(testId);
            var currentQuestion = _schoolDBContext.Questions
                .Include(qa => qa.QuestionsAnswers)
                .Where(questions => questions.Id == questionId)
                .First();
            if (currentTest == null || currentQuestion == null)
                throw new NullReferenceException();

            var questionAnswer = new List<string>();
            var correctAnser = string.Empty;

            foreach (var answers in currentQuestion.QuestionsAnswers)
            {
                if (answers.IsCorrect)
                    correctAnser = GetAnswerName(answers.AnswerId);
                else
                    questionAnswer.Add(GetAnswerName(answers.AnswerId));
            }

            if (questionAnswer.Count == 0)
                questionAnswer = null;

            return new CreateQuestionTransferObject
            {
                Test = currentTest,
                QuestionName = currentQuestion.Name,
                Points = currentQuestion.Points,
                CorrectAnswer = correctAnser,
                Answers = questionAnswer,
                QuestionId = questionId
            };
        }

        private bool UpdateAnswer(string? answerId, string newName)
        {
            var answer = _schoolDBContext.Answers.Find(answerId);
            if (answer == null)
                return true;
            answer.Name = newName;
            return false;
        }

        public bool UpdateQuestion(string? questionId, CreateQuestionTransferObject transferObject)
        {
            var currentQuestion = _schoolDBContext.Questions
                .Include(qa => qa.QuestionsAnswers)
                .Where(questions => questions.Id == questionId)
                .First();
            if (currentQuestion == null || transferObject == null)
                return true;

            currentQuestion.Name = transferObject.QuestionName;
            currentQuestion.Points = transferObject.Points;

            foreach (var questionAnswer in currentQuestion.QuestionsAnswers)
            {
                if (questionAnswer.IsCorrect)
                {
                    if (UpdateAnswer(questionAnswer.AnswerId, transferObject.CorrectAnswer))
                        return true;
                    break;
                }
            }

            int counterAnswer = 0;
            foreach (var questionAnswer in currentQuestion.QuestionsAnswers)
            {
                if (questionAnswer.IsCorrect) { }
                else
                {
                    if (transferObject.Answers?[counterAnswer] == null)
                    {
                        _schoolDBContext.Remove(questionAnswer);
                        if (DeleteAnswer(questionAnswer.AnswerId))
                            return true;
                        counterAnswer++;
                    }
                    else
                    {
                        if (UpdateAnswer(questionAnswer.AnswerId, transferObject.Answers[counterAnswer]))
                            return true;
                        else
                            counterAnswer++;
                    }
                }
            }
            _schoolDBContext.SaveChanges();
            List<string> answers= new List<string>();
            foreach (var answer in transferObject.Answers ?? new List<string>())
            {
                if (answer == null)
                    break;
                else
                    answers.Add(answer);
            }
            int counter = currentQuestion.QuestionsAnswers.Count - answers.Count;
            counter--;

            if (counter < 0)
            {
                counter = -counter;
                for (int i = 0; i < counter; i++)
                {
                    var answer = CreateAnswer(answers[counterAnswer]);
                    ConnectQuestionAnswer(currentQuestion.Id, answer.Id, false);
                    counterAnswer++;
                }
            }
            else if (counter == 0) { }

            _schoolDBContext.SaveChanges();
            return false;


        }

        public bool DeleteTest(string? testId)
        {
            var currentTest = _schoolDBContext.Tests
                .Include(q => q.Questions)
                .Include(ut => ut.UsersTests)
                .Include(cs => cs.CourseSections)
                .Where(tests => tests.Id == testId)
                .First();
            if (currentTest == null)
                return true;

            foreach (var users in currentTest.UsersTests)
            {
                _schoolDBContext.Remove(users);
            }

            foreach (var sections in currentTest.CourseSections)
            {
                currentTest.CourseSections.Remove(sections);
            }

            foreach (var question in currentTest.Questions)
            { 
                if (DeleteQuestion(currentTest.Id, question.Id))
                    return true;
            }
            _schoolDBContext.Remove(currentTest);
            _schoolDBContext.SaveChanges();
            return false;
        }
    }
}
