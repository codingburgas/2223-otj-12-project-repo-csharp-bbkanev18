using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class File
    {
        public File()
        {
            Users = new HashSet<User>();
            CourseSections = new HashSet<CoursesSection>();
        }

        public string Id { get; set; } = null!;
        public string Filename { get; set; } = null!;
        public byte[] FileData { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<CoursesSection> CourseSections { get; set; }
    }
}
