using SchoolSystem.BLL.Services.interfaces;
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

        public void AddCourse(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
