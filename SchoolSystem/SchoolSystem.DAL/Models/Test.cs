using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class Test
    {
        public Test()
        {
            UsersTests = new HashSet<UsersTest>();
            CourseSections = new HashSet<CoursesSection>();
            Questions = new HashSet<Question>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int TimeLimit { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime DateOfCreated { get; set; }

        public virtual ICollection<UsersTest> UsersTests { get; set; }

        public virtual ICollection<CoursesSection> CourseSections { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
