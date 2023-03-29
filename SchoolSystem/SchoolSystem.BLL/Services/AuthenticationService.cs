using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public string LogIn(User user, SchoolDBContext dBContext)
        {
            throw new NotImplementedException();
        }

        public void Register(User user, SchoolDBContext dBContext)
        {
            throw new NotImplementedException();
        }
    }
}
