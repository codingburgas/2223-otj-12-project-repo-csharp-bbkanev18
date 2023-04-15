using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
            var tempCourse = TransferToCourse(course);

            if (CheckCourseName(tempCourse))
                return true;

            _schoolDBContext.Add(tempCourse);
            _schoolDBContext.SaveChanges();
            if (course?.SectionName?.Count()>0)
            {
                var courseId = GetCourseId(course);
                foreach (var sectionName in course.SectionName)
                {
                    if (sectionName != null)
                    {
                        _schoolDBContext.Add(TransferToCourseSection(sectionName, courseId));
                    }
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

        private bool CheckCourseName(Course course)
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

        public Course? GetCourseById(string? courseId)
        {
            return _schoolDBContext.Courses.Where(courses => courses.Id == courseId).FirstOrDefault();
        }

        public bool EditCourse(Course course)
        {
            if (CheckCourseName(course))
                return true;
            if (course.Name.Length<2 || course.Name.Length>250)
                return true;

            return false;
        }

        public void DetachingCourse(Course course)
        {
            _schoolDBContext.Entry(course).State = EntityState.Deleted;
        }

        public void AttachCourse(Course course)
        {
            _schoolDBContext.Entry(course).State|= EntityState.Modified;
        }

        public List<CourseSectionTransferObject> GetCourseSection(string? courseId)
        {
            var temp = new List<CourseSectionTransferObject>();
            var courseSections = _schoolDBContext.CoursesSections.ToList();
            var course = _schoolDBContext.Courses.Find(courseId);
            foreach (var section in courseSections)
            {
                if(section.CourseId == courseId)
                {
                    var TransferObject = new CourseSectionTransferObject
                    {
                        Id = section.Id,
                        Name = section.Name,
                        CourseId = courseId,
                        CourseName = course?.Name ?? string.Empty
                    };
                    temp.Add(TransferObject);
                }
            }

            if(temp.Count == 0)
                temp.Add(new CourseSectionTransferObject { CourseName = course?.Name ?? string.Empty });

            return temp;
        }
    }
}
