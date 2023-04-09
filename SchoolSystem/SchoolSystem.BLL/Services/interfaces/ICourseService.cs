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
    }
}
