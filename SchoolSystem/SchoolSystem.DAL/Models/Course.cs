using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class Course
    {
        public Course()
        {
            CoursesSections = new HashSet<CoursesSection>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<CoursesSection> CoursesSections { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
