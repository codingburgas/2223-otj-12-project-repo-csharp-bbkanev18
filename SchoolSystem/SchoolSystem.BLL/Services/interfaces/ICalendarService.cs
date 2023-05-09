using SchoolSystem.DAL.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface ICalendarService
    {
        public List<CalendarTransferObject> GetCalendarTransfers(string userId);
    }
}
