using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace SchoolSystem.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public ClaimsIdentity SignIn(UserSignInDataTransferObject user, SchoolDBContext dBContext)
        {
            var users = dBContext.Users.ToList();
            var userPassword = ComputeSha256Hash(user.Password);

            foreach (var item in users)
            {
                if (user.Email == item.Email && userPassword == item.Password)
                    return GenerateTokes(item,dBContext);
            }

            return new ClaimsIdentity();
        }

        public void SignUp(UserSignInDataTransferObject user, SchoolDBContext dBContext)
        {
            throw new NotImplementedException();
        }

        private string GetRole(string roleId, SchoolDBContext dBContext)
        {
            var roles = dBContext.Roles.ToList();

            foreach (var role in roles)
            {
                if (roleId == role.Id)
                    return role.Name;
            }
            return "user";
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

        private ClaimsIdentity GenerateTokes(User user, SchoolDBContext dBContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Role, GetRole(user.RoleId, dBContext))
            };

            return new ClaimsIdentity(claims, "login");
        }
    }
}
