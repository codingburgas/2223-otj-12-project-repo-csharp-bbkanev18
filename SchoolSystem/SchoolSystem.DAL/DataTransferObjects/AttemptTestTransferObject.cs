using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class AttemptTestTransferObject
    {
        public Question CurrentQuestion { get; set; } = new Question();
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public string TestId { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
    }
}
