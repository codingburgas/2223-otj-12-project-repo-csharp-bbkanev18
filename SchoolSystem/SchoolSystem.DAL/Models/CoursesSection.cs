using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class CoursesSection
    {
        public CoursesSection()
        {
            Files = new HashSet<File>();
            Tests = new HashSet<Test>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CourseId { get; set; } = null!;

        public virtual Course Course { get; set; } = null!;

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
