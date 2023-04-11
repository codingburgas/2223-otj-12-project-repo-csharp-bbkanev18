using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly SchoolDBContext _schoolDBContext;

        public AccountService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public UserSignUpDataTransferObject GetUserById(string id)
        {
            var user = _schoolDBContext.Users.Where(users => users.Id == id).FirstOrDefault();
            if (user == null)
                return new UserSignUpDataTransferObject();
            return UserToTransfer(user);
        }

        public bool UpdateUser(UserSignUpDataTransferObject user)
        {
            var existingUser = _schoolDBContext.Users.Find(user.Id);
            if(existingUser == null)
                return true;
            if (existingUser.Email == user.Email) { }
            else if(CheckEmail(existingUser))
                return true;
            SwapTransferToUser(user, existingUser);
            _schoolDBContext.Update(existingUser);
            _schoolDBContext.SaveChanges();
            return false;
        }

        private bool CheckEmail(User user)
        {
            return _schoolDBContext.Users.Where(users => users.Email == user.Email).Count() > 0;
        }

        private void SwapTransferToUser(UserSignUpDataTransferObject user, User userEntity)
        {
            userEntity.FirstName= user.FirstName;
            userEntity.MiddleName = user.MiddleName;
            userEntity.LastName = user.LastName;
            userEntity.Email = user.Email;
            userEntity.Age = user.Age;
            userEntity.Address = user.Address;
            userEntity.Phone = user.Phone;
        }

        private UserSignUpDataTransferObject UserToTransfer(User user)
        {
            return new UserSignUpDataTransferObject
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email,
                Password= user.Password,
                Age = user.Age,
                Address = user.Address,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
        }
    }
}
