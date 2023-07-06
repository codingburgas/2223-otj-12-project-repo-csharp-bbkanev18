using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class QuestionInTestTransferObject
    {
        public Test Test { get; set; } = new Test();
        public List<Question> Questions { get; set;} = new List<Question>();
    }
}
