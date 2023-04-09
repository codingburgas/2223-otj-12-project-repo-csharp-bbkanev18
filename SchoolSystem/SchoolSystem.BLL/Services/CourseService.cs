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
    public class CourseService : ICourseService
    {
        private readonly SchoolDBContext _schoolDBContext;

        public CourseService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public bool CreateCourse(CourseCreateTransferObject course)
        {
            if(CheckCourseName(course))
            {
                return true;
            }
            _schoolDBContext.Add(TransferToCourse(course));
            _schoolDBContext.SaveChanges();
            if (course?.SectionName?.Count()>0)
            {
                var courseId = GetCourseId(course);
                foreach (var sectionName in course.SectionName)
                {
                    if(sectionName != null)
                        _schoolDBContext.Add(TransferToCourseSection(sectionName, courseId));
                }
            }
            _schoolDBContext.SaveChanges();
            return false;
        }

        private Course TransferToCourse(CourseCreateTransferObject course)
        {
            return new Course
            {
                Name = course.Name
            };
        }

        private CoursesSection TransferToCourseSection(string sectionName, string courseId)
        {
            return new CoursesSection
            {
                Name = sectionName,
                CourseId = courseId
            };
        }

        private bool CheckCourseName(CourseCreateTransferObject course)
        {
            var courses = _schoolDBContext.Courses;
            foreach (var item in courses)
            {
                if(item.Name == course.Name)
                    return true;
            }
            return false;
        }

        private string GetCourseId(CourseCreateTransferObject course)
        {
            var courses = _schoolDBContext.Courses;
            foreach (var item in courses)
            {
                if (item.Name == course.Name)
                    return item.Id;
            }
            return string.Empty;
        }
    }
}
