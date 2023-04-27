using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
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

            if(currentUser?.UsersTests != null)
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

            currentTest.Name= transferObject.Name;
            currentTest.TimeLimit= transferObject.TimeLimit;
            currentTest.Deadline= transferObject.Deadline;

            _schoolDBContext.SaveChanges();

            return false;

        }
    }
}
