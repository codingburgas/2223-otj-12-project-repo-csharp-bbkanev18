using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class AddUserInCourseTransferObject
    {
        public Course Course { get; set; } = new Course();
        public List<User> Users { get; set; } = new List<User>();
    }
}
