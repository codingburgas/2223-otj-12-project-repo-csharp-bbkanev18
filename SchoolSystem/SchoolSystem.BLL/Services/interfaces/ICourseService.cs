using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface ICourseService
    {
        public bool CreateCourse(CourseCreateTransferObject course);

        public Course? GetCourseById(string? courseId);

        public bool EditCourse(Course course);

        public void DetachingCourse(Course course);

        public void AttachCourse(Course course);

        public List<CourseSectionTransferObject> GetCourseSections(string? courseId, string? currentUserId);
        public CourseSectionTransferObject GetCourseSection(string? courseId);

        public CourseSectionTransferObject GetSection(string? sectionId);

        public TestAddInSectionTransferObject GetTestAddInSectionTransferObject(string? sectionId);

        public FileAddInSectionTransferObject GetFileAddInSection(string? sectionId);

        public List<Course> GetCoursesUser(string? userId);

        public AddUserInCourseTransferObject GetAddUserInCourse(string? courseId);

        public AddUserInCourseTransferObject GetSignInUsers(string? courseId);

        public bool CreateTest(TestAddInSectionTransferObject transferObject);

        public bool CreateLesson(FileAddInSectionTransferObject? transferObject);
        
        public bool CreateSectionCourse(CourseSectionTransferObject transferObject);

        public bool UpdateSectionCourse(CourseSectionTransferObject transferObject);

        public bool AddUserInCourse(string courseId, string userId);

        
    }
}
