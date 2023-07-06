using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class CalendarTransferObject
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }
    }
}
