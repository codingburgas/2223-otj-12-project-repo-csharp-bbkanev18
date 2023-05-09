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
    public class CalendarService : ICalendarService
    {
        private readonly SchoolDBContext _schoolDBContext;
        public CalendarService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public List<CalendarTransferObject> GetCalendarTransfers(string userId)
        {
            var currentUser = _schoolDBContext.Users
                .Include(c => c.Courses)
                .Include(r => r.Role)
                .Where(users => users.Id == userId)
                .First();
            if (currentUser == null || userId == null)
                return new List<CalendarTransferObject>();

            var calendarEvents = new List<CalendarTransferObject>();

            if(currentUser.Role.Name == "admin" || currentUser.Role.Name == "teacher")
            {
                var Tests = _schoolDBContext.Tests.ToList();
                foreach (var test in Tests)
                {
                    var calendarEvent = GetCalendarEvent(test.Id);
                    if (calendarEvent == null)
                        return new List<CalendarTransferObject>();
                    calendarEvents.Add(calendarEvent);
                }
                return calendarEvents;
            }
            
            

            foreach (var course in currentUser.Courses)
            {
                var sections = GetCoursesSections(course.Id);
                if (sections == null)
                    return new List<CalendarTransferObject>();
                foreach (var section in sections)
                {
                    foreach (var test in section.Tests)
                    {
                        var calendarEvent = GetCalendarEvent(test.Id);
                        if (calendarEvent == null)
                            return new List<CalendarTransferObject>();
                        calendarEvents.Add(calendarEvent);
                    }
                }
            }

            return calendarEvents;
        }


        private List<CoursesSection> GetCoursesSections(string courseId)
        {
            var sections = _schoolDBContext.CoursesSections
                .Include(sct=>sct.Tests)
                .Where(sections=>sections.CourseId == courseId)
                .ToList();
            if(sections == null)
                return new List<CoursesSection>();
            return sections;
        }
        private CalendarTransferObject GetCalendarEvent(string testId)
        {
            var currentTest = _schoolDBContext.Tests.Find(testId);
            if(currentTest == null)
                return new CalendarTransferObject();

            return new CalendarTransferObject()
            {
                Name = currentTest.Name,
                Deadline = currentTest.Deadline
            };
        }
    }
}
