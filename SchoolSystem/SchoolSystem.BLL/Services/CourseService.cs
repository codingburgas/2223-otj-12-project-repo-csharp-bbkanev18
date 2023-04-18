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

        public List<CourseSectionTransferObject> GetCourseSections(string? courseId)
        {
            var temp = new List<CourseSectionTransferObject>();
            var TransferObject = new CourseSectionTransferObject();
            //var test = _schoolDBContext.CoursesSections.Include(t => t.Tests).ToList();
            /*
            var test = _schoolDBContext.Tests.FirstOrDefault();
            var book = _schoolDBContext.CoursesSections.Where(coures => coures.Name == "Test").FirstOrDefault();

            book?.Tests.Add(test);
            test?.CourseSections.Add(book);
            _schoolDBContext.SaveChanges();
            */
            var courseSections = _schoolDBContext.CoursesSections.Include(t => t.Tests).Include(f=>f.Files).ToList();
            var course = _schoolDBContext.Courses.Find(courseId);
            foreach (var section in courseSections)
            { 
                if(section.CourseId == courseId)
                {
                    TransferObject = new CourseSectionTransferObject()
                    {
                        Id = section.Id,
                        Name = section.Name,
                        CourseId = courseId ?? string.Empty,
                        CourseName = course?.Name ?? string.Empty
                    };

                    if (section.Tests.Count > 0)
                        foreach (var test in section.Tests)
                            TransferObject.Tests.Add(test);

                    if (section.Files.Count > 0)
                        foreach (var file in section.Files)
                            TransferObject.Files.Add(file);
                    temp.Add(TransferObject);
                }
            }

            if(temp.Count == 0)
                temp.Add(new CourseSectionTransferObject { CourseId = course?.Id ?? string.Empty,CourseName = course?.Name ?? string.Empty });

            return temp;
        }

        public CourseSectionTransferObject GetCourseSection(string? courseId)
        {
            var course = _schoolDBContext.Courses.Find(courseId);
            return new CourseSectionTransferObject
            {
                CourseId = courseId ?? string.Empty,
                CourseName = course?.Name ?? string.Empty
            };
        }

        public bool CreateSectionCourse(CourseSectionTransferObject transferObject)
        {
            if (transferObject == null)
                return true;

            var newCourseSectio = new CoursesSection();

            newCourseSectio.Name = transferObject.Name;
            newCourseSectio.CourseId = transferObject.Id;
            
            _schoolDBContext.Add(newCourseSectio);
            _schoolDBContext.SaveChanges();
            return false;
        }
    }
}
