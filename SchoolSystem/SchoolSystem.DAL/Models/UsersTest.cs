using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class UsersTest
    {
        public string TestId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int Score { get; set; }

        public virtual Test Test { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
