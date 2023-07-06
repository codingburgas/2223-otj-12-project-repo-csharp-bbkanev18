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
        private CourseService _courseService;
        [SetUp]
        public void Setup()
        {
            _schoolDBContext = new SchoolDBContext();
            _authenticationService = new AuthenticationService(_schoolDBContext);
            _courseService = new CourseService(_schoolDBContext);
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
                Email = "xixorav606@soombo.com",
                Password = "Test!1234",
                ConfirmPassword = "Test!1234",
                Age = 16
            };

            // Act
            _authenticationService.SignUp(user);

            // Assert
            User user1 = _schoolDBContext.Users.Where(user => user.Email == "xixorav606@soombo.com").First();
            Assert.That(user1 is not null);

            // Delete created user
            _schoolDBContext.Remove<User>(user1 ?? new User());
            _schoolDBContext.SaveChanges();
        }

        [Test]
        public void Should_CreateCourse_When_InvokeCreateCourseMethod()
        {
            // Arrange
            CourseCreateTransferObject newCourse = new CourseCreateTransferObject()
            {
                Name = "Test"
            };

            // Act
            _courseService.CreateCourse(newCourse);

            // Assert
            var course = _schoolDBContext.Courses.Where(courses => courses.Name == "Test").First();
            Assert.That(course is not null);

            // Delete created course
            _schoolDBContext.Remove<Course>(course ?? new Course());
            _schoolDBContext.SaveChanges();
        }
    }
}