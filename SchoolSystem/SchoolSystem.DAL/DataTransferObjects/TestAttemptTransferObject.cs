using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class TestAttemptTransferObject
    {
        public string Id { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public DateTime? Deadline { get; set; }
        public string CourseId { get; set; } = string.Empty;
        public bool IsCurrentUserMakeTest { get; set; }
        public int UserScore { get; set; }

    }
}
