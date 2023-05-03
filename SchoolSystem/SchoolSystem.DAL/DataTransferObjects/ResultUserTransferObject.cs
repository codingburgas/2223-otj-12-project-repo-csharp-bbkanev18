using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class ResultUserTransferObject
    {
        public Test CurrentTest { get; set; } = new Test();
        public Dictionary<User, int> Users { get; set; } = new Dictionary<User, int>();
    }
}
