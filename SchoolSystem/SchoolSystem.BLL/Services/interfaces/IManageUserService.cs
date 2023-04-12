using SchoolSystem.DAL.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.BLL.Services.interfaces
{
    public interface IManageUserService
    {
        public List<ManageUserTransferObject> GetManageUserTransferObjects();
        public ManageUserTransferObject GetManageUserTransferObjectById(string? id);
        public bool UpdateUser(ManageUserTransferObject user, string? role);

        public bool UpdateUserPassword(ManageUserTransferObject user);
    }
}
