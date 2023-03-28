using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class Answer
    {
        public Answer()
        {
            QuestionsAnswers = new HashSet<QuestionsAnswer>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<QuestionsAnswer> QuestionsAnswers { get; set; }
    }
}
