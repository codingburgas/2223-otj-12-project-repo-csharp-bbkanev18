using SchoolSystem.BLL.Services;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;

namespace SchoolSystem.Tests
{
    public class Tests
    {
        private SchoolDBContext _schoolDBContext;
        private AuthenticationService _authenticationService;
        [SetUp]
        public void Setup()
        {
            _schoolDBContext = new SchoolDBContext();
            _authenticationService = new AuthenticationService(_schoolDBContext);
        }

        [Test]
        public void Should_RegisterUser_When_InvokeSignUpMethod()
        {
            // Arrange
            UserSignUpDataTransferObject user = new UserSignUpDataTransferObject()
            {
                FirstName = "Boris",
                MiddleName = "Biserov",
                LastName = "Kanev",
                Email = "bbkanev19@codingburgas.bg",
                Password = "Test!1234",
                ConfirmPassword = "Test!1234",
                Age = 16
            };
            // Act
            _authenticationService.SignUp(user);
            // Assert
            User user1 = _schoolDBContext.Users.Where(user => user.Email == "bbkanev19@codingburgas.bg").First();
            Assert.That(user1 is not null);

            _schoolDBContext.Remove<User>(user1 ?? new User());
            _schoolDBContext.SaveChanges();
        }

    }
}