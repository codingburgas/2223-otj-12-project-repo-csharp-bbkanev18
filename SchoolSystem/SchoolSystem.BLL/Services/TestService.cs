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
            var test = _schoolDBContext.Tests.Include(cs => cs.CourseSections).Where(t => t.Id == testId).FirstOrDefault();
            var currentUser = _schoolDBContext.Users.Include(ut => ut.UsersTests).Where(cu => cu.Id == currentUserId).FirstOrDefault();
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
                CourseId = courseId
            };

        }
    }
}
