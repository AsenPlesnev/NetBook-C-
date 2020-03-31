using Microsoft.EntityFrameworkCore;

namespace NetBook.Services.Data.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Data.User;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class UserServiceTests
    {
        private IUserService userService;

        public UserServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<NetBookUser> GetDummyData()
        {
            return new List<NetBookUser>
            {
                new NetBookUser
                {
                    FullName = "TestUser",
                    Email = "test@email.com",
                    PhoneNumber = "TestPhone",
                    IsTeacher = false,
                },
                new NetBookUser
                {
                    FullName = "TestUser2",
                    Email = "test2@email.com",
                    PhoneNumber = "TestPhone2",
                    IsTeacher = true,
                    IsClassTeacher = false,
                },
                new NetBookUser
                {
                    FullName = "TestUser3",
                    Email = "test3@email.com",
                    PhoneNumber = "TestPhone3",
                    IsTeacher = true,
                    IsClassTeacher = false,
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllUsersWithRoles_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "UserService GetAllUsersWithRoles() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.userService = new UserService(context);

            var actualResult = await this.userService.GetAllUsersWithRoles().ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllUsersWithRoles_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "UserService GetAllUsersWithRoles() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var actualResult = await this.userService.GetAllUsersWithRoles().ToListAsync();
            var expectedResult = await context.Users.To<UserServiceModel>().ToListAsync();

            var users = context.Users.OrderBy(x => x.FullName);

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.FullName == expectedEntity.FullName, errorMessagePrefix + " " + "User Full Name is not returned properly.");
                Assert.True(actualEntity.Email == expectedEntity.Email, errorMessagePrefix + " " + "User Email is not returned properly.");
                Assert.True(actualEntity.PhoneNumber == expectedEntity.PhoneNumber, errorMessagePrefix + " " + "User Phone Number is not returned properly.");
            }
        }

        [Fact]
        public async Task GetUser_WithExistentId_ShouldSuccessfullyReturnUser()
        {
            string errorMessagePrefix = "UserService GetUserByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = context.Users.First().Id;

            var actualResult = await this.userService.GetUserByIdAsync(testId);
            var expectedResult = context.Users.First().To<UserServiceModel>();

            Assert.True(actualResult.FullName == expectedResult.FullName, errorMessagePrefix + " " + "User Full Name is not returned properly.");
            Assert.True(actualResult.Email == expectedResult.Email, errorMessagePrefix + " " + "User Email is not returned properly.");
            Assert.True(actualResult.PhoneNumber == expectedResult.PhoneNumber, errorMessagePrefix + " " + "User Phone Number is not returned properly.");
        }

        [Fact]
        public async Task GetUser_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "UserService GetUserByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.userService.GetUserByIdAsync(testId));
        }

        [Fact]
        public async Task DeleteUser_WithExistentId_ShouldSuccessfullyDeleteUser()
        {
            string errorMessagePrefix = "UserService DeleteUserAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = context.Users.First().Id;

            var actualResult = await this.userService.DeleteUserAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var deletedUser = context.Users.Find(testId);

            Assert.True(deletedUser.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly.");
        }

        [Fact]
        public async Task DeleteUser_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "UserService DeleteUserAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.userService.DeleteUserAsync(testId));
        }

        [Fact]
        public async Task GetTeacherNames_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "UserService GetTeacherNames() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.userService = new UserService(context);

            var actualResult = await this.userService.GetTeacherNames();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetTeacherNames_WithDummyDataAndNoClassTeachers_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "UserService GetTeacherNames() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var actualResult = await this.userService.GetTeacherNames();
            var expectedResult = new List<SelectListItem>();

            var teachers = context.Users.Where(x => x.IsTeacher);

            foreach (var teacher in teachers)
            {
                var item = new SelectListItem
                {
                    Text = teacher.FullName,
                    Value = teacher.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Text == expectedEntity.Text, errorMessagePrefix + " " + "Teacher Name is not returned properly.");
                Assert.True(actualEntity.Value == expectedEntity.Value, errorMessagePrefix + " " + "Teacher Id is not returned properly.");
            }
        }

        [Fact]
        public async Task GetTeacherNames_WithDummyDataAndClassTeachers_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "UserService GetTeacherNames() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var classTeacher = context.Users.First(x => x.IsTeacher);
            classTeacher.IsClassTeacher = true;

            context.Users.Update(classTeacher);
            await context.SaveChangesAsync();

            var actualResult = await this.userService.GetTeacherNames();
            var expectedResult = new List<SelectListItem>();

            var teachers = context.Users.Where(x => x.IsTeacher && !x.IsClassTeacher);

            foreach (var teacher in teachers)
            {
                var item = new SelectListItem
                {
                    Text = teacher.FullName,
                    Value = teacher.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Text == expectedEntity.Text, errorMessagePrefix + " " + "Teacher Name is not returned properly.");
                Assert.True(actualEntity.Value == expectedEntity.Value, errorMessagePrefix + " " + "Teacher Id is not returned properly.");
            }
        }

        [Fact]
        public async Task AddUserAsTeacher_WithExistentId_ShouldSuccessfullyAddUserAsTeacher()
        {
            string errorMessagePrefix = "UserService AddUserAsTeacherAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = context.Users.First(x => !x.IsTeacher).Id;

            var actualResult = await this.userService.AddUserAsTeacherAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var teacher = context.Users.Find(testId);

            Assert.True(teacher.IsTeacher, errorMessagePrefix + " " + "IsTeacher is not set properly.");
        }

        [Fact]
        public async Task AddUserAsTeacher_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "UserService AddUserAsTeacherAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.userService = new UserService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.userService.AddUserAsTeacherAsync(testId));
        }
    }
}
