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
        private readonly SchoolDBContext _schoolDBContext;
        public AuthenticationService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public ClaimsIdentity SignIn(UserSignInDataTransferObject user)
        {
            var users = _schoolDBContext.Users.ToList();
            var userPassword = ComputeSha256Hash(user.Password);

            foreach (var item in users)
            {
                if (user.Email == item.Email && userPassword == item.Password)
                    return GenerateTokes(item);
            }

            return new ClaimsIdentity();
        }

        public void SignUp(UserSignInDataTransferObject user)
        {
            throw new NotImplementedException();
        }
        public User GetUserById(string userId)
        {
            return _schoolDBContext.Users.Where(user => user.Id == userId).FirstOrDefault() ?? new User();
        }

        private string GetRole(string roleId)
        {
            var roles = _schoolDBContext.Roles.ToList();

            foreach (var role in roles)
            {
                if (roleId == role.Id)
                    return role.Name;
            }
            return "guest";
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


        private ClaimsIdentity GenerateTokes(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Role, GetRole(user.RoleId))
            };

            return new ClaimsIdentity(claims, "login");
        }
    }
}
