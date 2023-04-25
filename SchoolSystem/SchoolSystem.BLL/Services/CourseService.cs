using Microsoft.EntityFrameworkCore;
using SchoolSystem.BLL.Services.interfaces;
using SchoolSystem.DAL.DataTransferObjects;
using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SchoolSystem.BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolDBContext _schoolDBContext;

        public CourseService(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public bool CreateCourse(CourseCreateTransferObject course)
        {
            var tempCourse = TransferToCourse(course);

            if (CheckCourseName(tempCourse))
                return true;

            _schoolDBContext.Add(tempCourse);
            _schoolDBContext.SaveChanges();
            if (course?.SectionName?.Count() > 0)
            {
                var courseId = GetCourseId(course);
                foreach (var sectionName in course.SectionName)
                {
                    if (sectionName != null)
                    {
                        _schoolDBContext.Add(TransferToCourseSection(sectionName, courseId));
                    }
                }
            }
            _schoolDBContext.SaveChanges();
            return false;
        }
        private Course TransferToCourse(CourseCreateTransferObject course)
        {
            return new Course
            {
                Name = course.Name
            };
        }

        private CoursesSection TransferToCourseSection(string sectionName, string courseId)
        {
            return new CoursesSection
            {
                Name = sectionName,
                CourseId = courseId
            };
        }

        private bool CheckCourseName(Course course)
        {
            var courses = _schoolDBContext.Courses;
            foreach (var item in courses)
            {
                if (item.Name == course.Name)
                    return true;
            }
            return false;
        }

        private string GetCourseId(CourseCreateTransferObject course)
        {
            var courses = _schoolDBContext.Courses;
            foreach (var item in courses)
            {
                if (item.Name == course.Name)
                    return item.Id;
            }
            return string.Empty;
        }

        public Course? GetCourseById(string? courseId)
        {
            return _schoolDBContext.Courses.Where(courses => courses.Id == courseId).FirstOrDefault();
        }

        public bool EditCourse(Course course)
        {
            if (CheckCourseName(course))
                return true;
            if (course.Name.Length < 2 || course.Name.Length > 250)
                return true;

            return false;
        }

        public void DetachingCourse(Course course)
        {
            _schoolDBContext.Entry(course).State = EntityState.Deleted;
        }

        public void AttachCourse(Course course)
        {
            _schoolDBContext.Entry(course).State |= EntityState.Modified;
        }

        public List<CourseSectionTransferObject> GetCourseSections(string? courseId, string? currentUserId)
        {
            var currentUser = _schoolDBContext.Users
                .Include(c => c.Courses)
                .Where(users => users.Id == currentUserId)
                .First();

            var counter = 0;

            foreach (var signInCourse in currentUser.Courses)
                if (signInCourse.Id == courseId)
                    counter++;

            if (counter == 0)
                return new List<CourseSectionTransferObject>();



            var temp = new List<CourseSectionTransferObject>();
            var TransferObject = new CourseSectionTransferObject();
            //var test = _schoolDBContext.CoursesSections.Include(t => t.Tests).ToList();
            /*
            var test = _schoolDBContext.Tests.FirstOrDefault();
            var book = _schoolDBContext.CoursesSections.Where(coures => coures.Name == "Test").FirstOrDefault();

            book?.Tests.Add(test);
            test?.CourseSections.Add(book);
            _schoolDBContext.SaveChanges();
            */
            var courseSections = _schoolDBContext.CoursesSections.Include(t => t.Tests).Include(f => f.Files).ToList();
            var course = _schoolDBContext.Courses.Find(courseId);
            foreach (var section in courseSections)
            {
                if (section.CourseId == courseId)
                {
                    TransferObject = new CourseSectionTransferObject()
                    {
                        Id = section.Id,
                        Name = section.Name,
                        CourseId = courseId ?? string.Empty,
                        CourseName = course?.Name ?? string.Empty
                    };

                    if (section.Tests.Count > 0)
                        foreach (var test in section.Tests)
                            TransferObject.Tests.Add(test);

                    if (section.Files.Count > 0)
                        foreach (var file in section.Files)
                            TransferObject.Files.Add(file);
                    temp.Add(TransferObject);
                }
            }

            if (temp.Count == 0)
                temp.Add(new CourseSectionTransferObject { CourseId = course?.Id ?? string.Empty, CourseName = course?.Name ?? string.Empty });

            return temp;
        }

        public CourseSectionTransferObject GetCourseSection(string? courseId)
        {
            var course = _schoolDBContext.Courses.Find(courseId);
            return new CourseSectionTransferObject
            {
                CourseId = courseId ?? string.Empty,
                CourseName = course?.Name ?? string.Empty
            };
        }

        public bool CreateSectionCourse(CourseSectionTransferObject transferObject)
        {
            if (transferObject == null)
                return true;

            var newCourseSectio = new CoursesSection();

            newCourseSectio.Name = transferObject.Name;
            newCourseSectio.CourseId = transferObject.Id;

            _schoolDBContext.Add(newCourseSectio);
            _schoolDBContext.SaveChanges();
            return false;
        }

        public CourseSectionTransferObject GetSection(string? sectionId)
        {
            var section = _schoolDBContext.CoursesSections.Find(sectionId);
            return new CourseSectionTransferObject
            {
                Id = sectionId ?? string.Empty,
                Name = section?.Name ?? string.Empty,
                CourseId = section?.CourseId ?? string.Empty,
            };
        }

        public bool UpdateSectionCourse(CourseSectionTransferObject transferObject)
        {
            var existingSection = _schoolDBContext.CoursesSections.Find(transferObject.Id);
            if (existingSection == null)
                return true;
            existingSection.Name = transferObject.Name;
            _schoolDBContext.Update(existingSection);
            _schoolDBContext.SaveChanges();
            return false;
        }

        public TestAddInSectionTransferObject GetTestAddInSectionTransferObject(string? sectionId)
        {
            var section = _schoolDBContext.CoursesSections.Find(sectionId);
            return new TestAddInSectionTransferObject
            {
                SectionId = sectionId ?? string.Empty,
                SectionName = section?.Name ?? string.Empty,
                CourseId = section?.CourseId ?? string.Empty
            };
        }

        public bool CreateTest(TestAddInSectionTransferObject transferObject)
        {
            var section = _schoolDBContext.CoursesSections.Find(transferObject?.Id);
            if (section == null)
                return true;

            var newTest = new Test();

            newTest.Name = transferObject?.Name ?? string.Empty;
            newTest.TimeLimit = transferObject?.TimeLimit ?? 1;
            newTest.Deadline = transferObject?.Deadline;
            _schoolDBContext.Add(newTest);
            _schoolDBContext.SaveChanges();

            section.Tests.Add(newTest);
            newTest.CourseSections.Add(section);

            _schoolDBContext.SaveChanges();

            return false;


        }

        public FileAddInSectionTransferObject GetFileAddInSection(string? sectionId)
        {
            var section = _schoolDBContext.CoursesSections.Find(sectionId);
            return new FileAddInSectionTransferObject
            {
                SectionId = sectionId ?? string.Empty,
                SectionName = section?.Name ?? string.Empty,
                CourseId = section?.CourseId ?? string.Empty
            };
        }

        public bool CreateLesson(FileAddInSectionTransferObject? transferObject)
        {
            var section = _schoolDBContext.CoursesSections.Find(transferObject?.Id);
            if (section == null)
                return true;

            var newFile = new DAL.Models.File();

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                transferObject?.File?.CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }

            newFile.Filename = transferObject?.File?.FileName ?? string.Empty;
            newFile.FileData = fileData;
            _schoolDBContext.Add(newFile);
            _schoolDBContext.SaveChanges();

            section.Files.Add(newFile);
            newFile.CourseSections.Add(section);

            _schoolDBContext.SaveChanges();

            return false;

        }

        public bool AddUserInCourse(string courseId, string userId)
        {
            var user = _schoolDBContext.Users.Include(c => c.Courses)
                .Where(users => users.Id == userId).FirstOrDefault();
            if (CheckUserCourse(courseId, user ?? new User()))
                return true;
            var course = _schoolDBContext.Courses.Include(u => u.Users)
                .Where(courses => courses.Id == courseId).FirstOrDefault();

            user?.Courses.Add(course ?? new Course());
            course?.Users.Add(user ?? new User());
            _schoolDBContext.SaveChanges();
            return false;

        }

        private bool CheckUserCourse(string courseId, User user)
        {
            foreach (var course in user.Courses)
            {
                if (course.Id == courseId)
                    return true;
            }
            return false;
        }

        public List<Course> GetCoursesUser(string? userId)
        {
            if (userId == null)
                return new List<Course>();

            var courses = new List<Course>();
            var user = _schoolDBContext.Users.Include(c => c.Courses)
                .Where(users => users.Id == userId).First();

            var role = _schoolDBContext.Roles.Find(user.RoleId);

            if (role?.Name == "admin" || role?.Name == "teacher")
                return _schoolDBContext.Courses.ToList();


            foreach (var course in user.Courses)
            {
                if (course == null)
                    break;
                courses.Add(course);
            }

            return courses;
        }

        public AddUserInCourseTransferObject GetAddUserInCourse(string? courseId)
        {
            if (courseId == null)
                return new AddUserInCourseTransferObject();


            var course = _schoolDBContext.Courses.Find(courseId);
            var users = _schoolDBContext.Users.Include(c => c.Courses).ToList();
            var notSignInUsers = _schoolDBContext.Users.Include(c => c.Courses).ToList();



            foreach (var user in users)
            {
                var role = _schoolDBContext.Roles.Find(user.RoleId);

                if (role?.Name == "admin" || role?.Name == "teacher")
                    notSignInUsers.Remove(user);

                foreach (var userCourse in user.Courses)
                {
                    if (userCourse.Id == courseId)
                        notSignInUsers.Remove(user);
                }
            }
            return new AddUserInCourseTransferObject()
            {
                Course = course ?? new Course(),
                Users = notSignInUsers
            };
        }
    }
}
