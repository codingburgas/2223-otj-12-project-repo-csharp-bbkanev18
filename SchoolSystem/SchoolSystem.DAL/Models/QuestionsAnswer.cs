using System;
using System.Collections.Generic;

namespace SchoolSystem.DAL.Models
{
    public partial class QuestionsAnswer
    {
        public string QuestionId { get; set; } = null!;
        public string AnswerId { get; set; } = null!;
        public bool IsCorrect { get; set; }

        public virtual Answer Answer { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
