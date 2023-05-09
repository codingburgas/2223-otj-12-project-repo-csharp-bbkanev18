using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly SchoolDBContext _schoolDBContext;
        public CalendarService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }
    }
}
