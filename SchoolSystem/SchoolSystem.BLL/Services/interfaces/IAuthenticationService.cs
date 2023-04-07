using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface IAuthenticationService
    {
        public bool SignUp(UserSignUpDataTransferObject user);
        public ClaimsIdentity SignIn(UserSignInDataTransferObject user);

        public User GetUserById(string userId);
    }
}
