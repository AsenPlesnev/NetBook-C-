namespace NetBook.Services.Data.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Data.Absence;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class AbsenceServiceTests
    {
        private IAbsenceService absenceService;

        public AbsenceServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Absence> GetDummyAbsenceData()
        {
            return new List<Absence>
            {
                new Absence
                {
                    Student = new Student
                        { FullName = "Test Student1", Class = new Class { ClassNumber = 11, ClassLetter = ClassLetter.Б } },
                    Subject = new SubjectClass { Subject = new Subject { Name = "Test Subject1" } },
                },
                new Absence
                {
                    Student = new Student
                        { FullName = "Test Student2", Class = new Class { ClassNumber = 12, ClassLetter = ClassLetter.A } },
                    Subject = new SubjectClass { Subject = new Subject { Name = "Test Subject2" } },
                },
                new Absence
                {
                    Student = new Student
                        { FullName = "Test Student3", Class = new Class { ClassNumber = 10, ClassLetter = ClassLetter.В } },
                    Subject = new SubjectClass { Subject = new Subject { Name = "Test Subject3" } },
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyAbsenceData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAbsences_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "AbsenceService GetAllAbsences() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.absenceService = new AbsenceService(context);

            List<AbsenceServiceModel> actualResult = await this.absenceService.GetAllAbsences().ToListAsync();
            int expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllAbsences_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AbsenceService GetAllAbsences() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.absenceService = new AbsenceService(context);

            List<AbsenceServiceModel> actualResult = await this.absenceService.GetAllAbsences().ToListAsync();
            List<AbsenceServiceModel> expectedResult = context.Absences.To<AbsenceServiceModel>().ToList();

            Assert.Equal(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntity = expectedResult[i];
                var actualEntity = actualResult[i];

                Assert.True(expectedEntity.Student.FullName == actualEntity.Student.FullName, errorMessagePrefix + " " + "FullName is not returned properly");
                Assert.True($"{expectedEntity.Student.Class.ClassNumber} {expectedEntity.Student.Class.ClassLetter}" == $"{actualEntity.Student.Class.ClassNumber} {actualEntity.Student.Class.ClassLetter}", errorMessagePrefix + " " + "Class Name is not returned properly");
                Assert.True(expectedEntity.Subject.Subject.Name == actualEntity.Subject.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly");
            }
        }

        [Fact]
        public async Task CreateAbsence_WithNonExistentStudent_ShouldReturnArgumentNullException()
        {
            string errorMessagePrefix = "AbsenceService CreateAbsenceAsync() method does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.absenceService = new AbsenceService(context);

            AbsenceServiceModel testAbsence = new AbsenceServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.absenceService.CreateAbsenceAsync(testAbsence));
        }

        [Fact]
        public async Task CreateAbsence_WithCorrectData_ShouldSuccessfullyCreateOrder()
        {
            string errorMessagePrefix = "AbsenceService CreateAbsenceAsync() method does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.absenceService = new AbsenceService(context);

            var student = new Student
            {
                Id = "test",
                FullName = "Test",
                Absences = new List<Absence>(),
            };

            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            AbsenceServiceModel testAbsence = new AbsenceServiceModel
            {
                StudentId = student.Id,
                Student = student.To<StudentServiceModel>(),
            };

            bool actualResult = await this.absenceService.CreateAbsenceAsync(testAbsence);

            var updatedStudent = context.Students.First();
            var expectedAbsencesCount = 1;

            Assert.True(actualResult, errorMessagePrefix);
            Assert.Equal(updatedStudent.Absences.Count, expectedAbsencesCount);
        }

        [Fact]
        public async Task DeleteAbsence_WithExistentId_ShouldSuccessfullyDeleteAbsence()
        {
            string errorMessagePrefix = "AbsenceService DeleteAbsenceAsync() method does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.absenceService = new AbsenceService(context);

            string testId = context.Absences.First().Id;

            await this.absenceService.DeleteAbsenceAsync(testId);

            Absence testAbsence = context.Absences.Find(testId);

            Assert.True(testAbsence.IsDeleted, errorMessagePrefix);
        }

        [Fact]
        public async Task DeleteAbsence_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "AbsenceService DeleteAbsenceAsync() method does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.absenceService = new AbsenceService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.absenceService.DeleteAbsenceAsync(testId));
        }

    }
}
