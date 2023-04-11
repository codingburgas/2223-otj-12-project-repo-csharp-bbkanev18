using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface IAccountService
    {
        public UserSignUpDataTransferObject GetUserById(string id);

        public bool UpdateUser(UserSignUpDataTransferObject user);
    }
}
