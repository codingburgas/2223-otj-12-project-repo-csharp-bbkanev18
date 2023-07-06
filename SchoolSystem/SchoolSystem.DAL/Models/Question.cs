using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class Question
    {
        public Question()
        {
            QuestionsAnswers = new HashSet<QuestionsAnswer>();
            Tests = new HashSet<Test>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Points { get; set; }
        public DateTime DateOfCreated { get; set; }

        public virtual ICollection<QuestionsAnswer> QuestionsAnswers { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}
