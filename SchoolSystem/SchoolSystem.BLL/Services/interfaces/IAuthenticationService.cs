using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface IAuthenticationService
    {
        public void Register(User user, SchoolDBContext dBContext);
        public string LogIn(User user, SchoolDBContext dBContext);
    }
}
