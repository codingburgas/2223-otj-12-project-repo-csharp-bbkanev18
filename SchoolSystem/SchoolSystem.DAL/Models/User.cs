using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class User
    {
        public User()
        {
            UsersTests = new HashSet<UsersTest>();
            Courses = new HashSet<Course>();
        }

        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public byte Age { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string RoleId { get; set; } = null!;
        public string? FileId { get; set; }

        public virtual File? File { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<UsersTest> UsersTests { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
