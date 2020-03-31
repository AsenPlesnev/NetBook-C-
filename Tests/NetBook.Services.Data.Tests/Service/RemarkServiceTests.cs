namespace NetBook.Services.Data.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Data.Remark;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class RemarkServiceTests
    {
        private IRemarkService remarkService;

        public RemarkServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Remark> GetDummyData()
        {
            return new List<Remark>
            {
                new Remark
                {
                    Student = new Student { Id = "TestStudent", FullName = "TestStudent", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Id = "TestSubject", Subject = new Subject { Name = "TestSubject" }, Workload = 125 },
                    Text = "TestText",
                },
                new Remark
                {
                    Student = new Student { Id = "TestStudent2", FullName = "TestStudent2", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Id = "TestSubject2", Subject = new Subject { Name = "TestSubject2" }, Workload = 125 },
                    Text = "TestText2",
                },
                new Remark
                {
                    Student = new Student { Id = "TestStudent3", FullName = "TestStudent3", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Id = "TestSubject3", Subject = new Subject { Name = "TestSubject3" }, Workload = 125 },
                    Text = "TestText3",
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllRemarks_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "RemarkService GetAllRemarks() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.remarkService = new RemarkService(context);

            var actualResult = await this.remarkService.GetAllRemarks().ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllRemarks_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "RemarkService GetAllRemarks() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var actualResult = await this.remarkService.GetAllRemarks().ToListAsync();
            var expectedResult = await context.Remarks.To<RemarkServiceModel>().ToListAsync();

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Text == expectedEntity.Text, errorMessagePrefix + " " + "Text is not returned properly.");
                Assert.True(actualEntity.Student.FullName == expectedEntity.Student.FullName, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
                Assert.True(actualEntity.Subject.Subject.Name == expectedEntity.Subject.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRemark_WithExistentId_ShouldSuccessfullyReturnRemark()
        {
            string errorMessagePrefix = "RemarkService GetRemarkByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var testId = context.Remarks.First().Id;

            var actualResult = await this.remarkService.GetRemarkByIdAsync(testId);
            var expectedResult = context.Remarks.First().To<RemarkServiceModel>();

            Assert.True(actualResult.Text == expectedResult.Text, errorMessagePrefix + " " + "Text is not returned properly.");
            Assert.True(actualResult.Student.FullName == expectedResult.Student.FullName, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
            Assert.True(actualResult.Subject.Subject.Name == expectedResult.Subject.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
        }

        [Fact]
        public async Task GetRemark_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "RemarkService GetRemarkByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.remarkService.GetRemarkByIdAsync(testId));
        }

        [Fact]
        public async Task CreateRemark_WithCorrectData_ShouldSuccessfullyCreateRemark()
        {
            string errorMessagePrefix = "RemarkService CreateRemarkAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var student = new Student { Id = "CreateRemark", Remarks = new List<Remark>(), Class = new Class() };

            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            var testRemark = new RemarkServiceModel { Id = "TestRemark", StudentId = student.Id, Student = student.To<StudentServiceModel>() };

            var actualResult = await this.remarkService.CreateRemarkAsync(testRemark);
            var studentRemarks = context.Students.Find(student.Id).Remarks;

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(studentRemarks.Contains(context.Remarks.Find(testRemark.Id)), errorMessagePrefix + " " + "Remark is not added to student.");
        }

        [Fact]
        public async Task CreateRemark_WithNonExistentStudent_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "RemarkService CreateRemarkAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var testRemark = new RemarkServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.remarkService.CreateRemarkAsync(testRemark));
        }

        [Fact]
        public async Task DeleteRemark_WithExistentId_ShouldSuccessfullyDeleteRemark()
        {
            string errorMessagePrefix = "RemarkService DeleteRemarkAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var testId = context.Remarks.First().Id;

            var actualResult = await this.remarkService.DeleteRemarkAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var deletedRemark = context.Remarks.Find(testId);

            Assert.True(deletedRemark.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly.");
        }

        [Fact]
        public async Task DeleteRemark_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "RemarkService DeleteRemarkAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.remarkService = new RemarkService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.remarkService.DeleteRemarkAsync(testId));
        }
    }
}
