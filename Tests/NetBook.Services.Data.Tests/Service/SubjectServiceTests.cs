namespace NetBook.Services.Data.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Data.Subject;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class SubjectServiceTests
    {
        private ISubjectService subjectService;

        public SubjectServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Subject> GetDummyData()
        {
            return new List<Subject>
            {
                new Subject
                {
                    Id = "TestSubject",
                    Name = "TestSubject",
                },
                new Subject
                {
                    Name = "TestSubject2",
                },
                new Subject
                {
                    Name = "TestSubject3",
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllSubjects_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "SubjectService GetAllSubjects() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.subjectService = new SubjectService(context);

            var actualResult = await this.subjectService.GetAllSubjects().ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllSubjects_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "SubjectService GetAllSubjects() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var actualResult = await this.subjectService.GetAllSubjects().ToListAsync();
            var expectedResult = await context.Subjects.OrderBy(x => x.Name).To<SubjectServiceModel>().ToListAsync();

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Name == expectedEntity.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetSubject_WithExistentId_ShouldSuccessfullyReturnSubject()
        {
            string errorMessagePrefix = "SubjectService GetSubjectByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = context.Subjects.First().Id;

            var actualResult = await this.subjectService.GetSubjectByIdAsync(testId);
            var expectedResult = context.Subjects.First().To<SubjectServiceModel>();

            Assert.True(actualResult.Name == expectedResult.Name, errorMessagePrefix);
        }

        [Fact]
        public async Task GetSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "SubjectService GetSubjectByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.subjectService.GetSubjectByIdAsync(testId));
        }

        [Fact]
        public async Task GetSubjectNames_WithExistentClassIdAndZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "SubjectService GetSubjectNamesAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.subjectService = new SubjectService(context);

            var testClass = new Class
            {
                Id = "TestClass",
                Subjects = new List<SubjectClass>(),
            };

            await context.Classes.AddAsync(testClass);
            await context.SaveChangesAsync();

            var actualResult = await this.subjectService.GetSubjectNamesAsync(testClass.Id);
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetSubjectNames_WithExistentClassIdAndDummyDataAndEmptyClass_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "SubjectService GetSubjectNamesAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testClass = new Class
            {
                Id = "TestClass",
                Subjects = new List<SubjectClass>(),
            };

            await context.Classes.AddAsync(testClass);
            await context.SaveChangesAsync();

            var actualResult = await this.subjectService.GetSubjectNamesAsync(testClass.Id);
            var expectedResult = new List<SelectListItem>();

            var subjects = context.Subjects;

            foreach (var subject in subjects)
            {
                var item = new SelectListItem
                {
                    Text = subject.Name,
                    Value = subject.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Text == expectedEntity.Text, errorMessagePrefix + " " + "Subject Name is not returned properly.");
                Assert.True(actualEntity.Value == expectedEntity.Value, errorMessagePrefix + " " + "Subject Id is not returned properly.");
            }
        }

        [Fact]
        public async Task GetSubjectNames_WithExistentClassIdAndDummyDataAndNotEmptyClass_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "SubjectService GetSubjectNamesAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testClass = new Class
            {
                Id = "TestClass",
                Subjects = new List<SubjectClass>
                {
                    new SubjectClass
                    {
                        SubjectId = "TestSubject",
                    },
                },
            };

            await context.Classes.AddAsync(testClass);
            await context.SaveChangesAsync();

            var actualResult = await this.subjectService.GetSubjectNamesAsync(testClass.Id);
            var expectedResult = new List<SelectListItem>();

            var subjects = context.Subjects;

            foreach (var subject in subjects)
            {
                var item = new SelectListItem
                {
                    Text = subject.Name,
                    Value = subject.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count - 1, errorMessagePrefix + " " + "Subject that is already present shouldn't be listed.");
        }

        [Fact]
        public async Task GetSubjectNames_WithNonExistentClassId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "SubjectService GetSubjectNamesAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.subjectService.GetSubjectNamesAsync(testId));
        }

        [Fact]
        public async Task GetSubjectsDropdown_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "SubjectService GetSubjectsDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.subjectService = new SubjectService(context);

            var actualResult = await this.subjectService.GetSubjectsDropdownAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetSubjectsDropdown_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "SubjectService GetSubjectsDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var actualResult = await this.subjectService.GetSubjectsDropdownAsync();
            var expectedResult = new List<SelectListItem>();

            var subjects = context.Subjects;

            foreach (var subject in subjects)
            {
                var item = new SelectListItem
                {
                    Text = subject.Name,
                    Value = subject.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Text == expectedEntity.Text, errorMessagePrefix + " " + "Subject Name is not returned properly.");
                Assert.True(actualEntity.Value == expectedEntity.Value, errorMessagePrefix + " " + "Subject Id is not returned properly.");
            }
        }

        [Fact]
        public async Task CreateSubject_WithCorrectData_ShouldSuccessfullyCreateSubject()
        {
            string errorMessagePrefix = "SubjectService CreateSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testSubject = new SubjectServiceModel();

            var actualResult = await this.subjectService.CreateSubjectAsync(testSubject);

            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task CreateSubject_WithSameSubjectName_ShouldReturnFalse()
        {
            string errorMessagePrefix = "SubjectService CreateSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testSubject = new SubjectServiceModel { Name = "TestSubject" };

            var actualResult = await this.subjectService.CreateSubjectAsync(testSubject);

            Assert.True(!actualResult, errorMessagePrefix + " " + "Subject with same name can not be created.");
        }

        [Fact]
        public async Task EditSubject_WithExistentIdAndCorrectData_ShouldSuccessfullyEditSubject()
        {
            string errorMessagePrefix = "SubjectService EditSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = context.Subjects.First().Id;

            var testSubject = new SubjectServiceModel
            {
                Id = testId,
                Name = "EditSubject",
            };

            var actualResult = await this.subjectService.EditSubjectAsync(testSubject);
            var editedSubject = context.Subjects.First();

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(editedSubject.Name == testSubject.Name, errorMessagePrefix + " " + "Subject Name is not set properly.");
        }

        [Fact]
        public async Task EditSubject_WithExistentIdAndSameSubjectName_ShouldReturnFalse()
        {
            string errorMessagePrefix = "SubjectService EditSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = context.Subjects.First().Id;

            var testSubject = new SubjectServiceModel
            {
                Id = testId,
                Name = "TestSubject2",
            };

            var actualResult = await this.subjectService.EditSubjectAsync(testSubject);

            Assert.True(!actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task EditSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "SubjectService EditSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = "Non_Existent";

            var testSubject = new SubjectServiceModel { Id = testId };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.subjectService.EditSubjectAsync(testSubject));
        }

        [Fact]
        public async Task DeleteSubject_WithExistentId_ShouldSuccessfullyDeleteSubject()
        {
            string errorMessagePrefix = "SubjectService DeleteSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = context.Subjects.First().Id;

            var actualResult = await this.subjectService.DeleteSubjectAsync(testId);
            var deletedSubject = context.Subjects.Find(testId);

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(deletedSubject.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly.");
        }

        [Fact]
        public async Task DeleteSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "SubjectService DeleteSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.subjectService = new SubjectService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.subjectService.DeleteSubjectAsync(testId));
        }
    }
}
