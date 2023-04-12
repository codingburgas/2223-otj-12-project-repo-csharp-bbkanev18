using Microsoft.EntityFrameworkCore.ValueGeneration;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services
{
    public class ManageUserService : IManageUserService
    {
        private readonly SchoolDBContext _schoolDBContext;
        public ManageUserService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public ManageUserTransferObject GetManageUserTransferObjectById(string? id)
        {
            var models = GetManageUserTransferObjects();
            foreach (var model in models)
                if (model.Id == id)
                    return model;
            return new ManageUserTransferObject();
        }

        public List<ManageUserTransferObject> GetManageUserTransferObjects()
        {
            var users = _schoolDBContext.Users.ToList();
            var roles = _schoolDBContext.Roles.ToList();

            var model = new List<ManageUserTransferObject>();

            foreach (var user in users)
            {
                foreach (var role in roles)
                {
                    if (user.RoleId == role.Id)
                    {
                        var subModel = new ManageUserTransferObject
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            MiddleName = user.LastName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Password = user.Password,
                            Age = user.Age,
                            Address = user.Address,
                            Phone = user.Phone,
                            RoleId = role.Id,
                            RoleName = role.Name,
                        };
                        model.Add(subModel);
                    }
                }
            }
            return model;
        }

        public bool UpdateUser(ManageUserTransferObject user, string? role)
        {
            var currentUser = _schoolDBContext.Users.Find(user.Id);
            if (currentUser == null)
                return true;
            if (currentUser.Email == user.Email) { }
            else if (CheckEmail(user.Email))
                return true;
            if (role == null)
                return true;
            else
                currentUser.RoleId = role;
            SwapUser(currentUser, user);
            _schoolDBContext.Update(currentUser);
            _schoolDBContext.SaveChanges();
            return false;
        }

        private void SwapUser(User currentUser, ManageUserTransferObject newUser)
        {
            currentUser.FirstName = newUser.FirstName;
            currentUser.MiddleName= newUser.MiddleName;
            currentUser.LastName = newUser.LastName;
            currentUser.Email = newUser.Email;
            currentUser.Age = newUser.Age;
            currentUser.Address = newUser.Address;
            currentUser.Phone = newUser.Phone;
        }

        private bool CheckEmail(string email)
        {
            return _schoolDBContext.Users.Where(users => users.Email == email).Count() > 0;
        }

        public bool UpdateUserPassword(ManageUserTransferObject user)
        {
            var currentUser = _schoolDBContext.Users.Find(user.Id);
            var newPassword = ComputeSha256Hash(user.Password);
            if (currentUser == null)
                return true;
            if (currentUser.Password == newPassword)
                return true;
            currentUser.Password = newPassword;
            user.Email = currentUser.Email;
            _schoolDBContext.Update(currentUser);
            _schoolDBContext.SaveChanges();
            return false;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
