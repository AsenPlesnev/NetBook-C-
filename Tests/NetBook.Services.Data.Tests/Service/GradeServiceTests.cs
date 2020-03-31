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
    using NetBook.Services.Data.Grade;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class GradeServiceTests
    {
        private IGradeService gradeService;

        public GradeServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Grade> GetDummyData()
        {
            return new List<Grade>
            {
                new Grade
                {
                    Id = "TestGrade",
                    Student = new Student { Id = "TestStudent", FullName = "TestStudent", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Id = "TestSubject", Subject = new Subject { Name = "TestSubject" }, Workload = 125 },
                    GradeValue = "5",
                    Term = Term.Първи,
                    IsTermGrade = false,
                },
                new Grade
                {
                    StudentId = "TestStudent",
                    SubjectId = "TestSubject",
                    GradeValue = "5",
                    Term = Term.Първи,
                    IsTermGrade = true,
                },
                new Grade
                {
                    Student = new Student { FullName = "TestStudent2", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Subject = new Subject { Name = "TestSubject2" }, Workload = 75 },
                    GradeValue = "6",
                    Term = Term.Втори,
                    IsTermGrade = false,
                },
                new Grade
                {
                    Student = new Student { FullName = "TestStudent3", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Subject = new Subject { Name = "TestSubject3" }, Workload = 225 },
                    GradeValue = "4",
                    Term = Term.Първи,
                    IsTermGrade = true,
                },
                new Grade
                {
                    Student = new Student { FullName = "TestStudent4", Grades = new List<Grade>() },
                    Subject = new SubjectClass { Subject = new Subject { Name = "TestSubject4" }, Workload = 225 },
                    GradeValue = "5",
                    Term = Term.Втори,
                    IsTermGrade = true,
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllGrades_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "GradeService GetAllGrades() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.gradeService = new GradeService(context);

            var actualResult = await this.gradeService.GetAllGrades().ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllGrades_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "GradeService GetAllGrades() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var actualResult = await this.gradeService.GetAllGrades().ToListAsync();
            var expectedResult = await context.Grades.To<GradeServiceModel>().ToListAsync();

            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(expectedEntity.GradeValue == actualEntity.GradeValue, errorMessagePrefix + " " + "Grade Value is not returned properly.");
                Assert.True(expectedEntity.Term == actualEntity.Term, errorMessagePrefix + " " + "Term is not returned properly.");
                Assert.True(expectedEntity.IsTermGrade == actualEntity.IsTermGrade, errorMessagePrefix + " " + "IsTermGrade is not returned properly.");
                Assert.True(expectedEntity.Subject.Subject.Name == actualEntity.Subject.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
                Assert.True(expectedEntity.Student.FullName == actualEntity.Student.FullName, errorMessagePrefix + " " + "Student FullName is not returned properly.");
            }
        }

        [Fact]
        public async Task GetGradesForSubject_WithExistentSubjectIdAndStudentIdAndZeroData_ShouldReturnZeroResults()
        {
            string errorMessagePrefix = "GradeService GetGradesForSubject() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var student = new Student { Id = "TestStudentWithNoGrades", FullName = "TestStudentWithZeroGrades", Grades = new List<Grade>() };
            var testClass = new Class
            {
                Id = "TestClass",
                Subjects = new List<SubjectClass>(),
            };

            var testSubject = new SubjectClass
            {
                Id = "TestSubjectWithNoGrades",
                Subject = new Subject
                {
                    Name = "TestSubject",
                },
            };

            testClass.Subjects.Add(testSubject);

            student.ClassId = "TestClass";
            student.Class = testClass;

            await context.Students.AddAsync(student);
            await context.Classes.AddAsync(testClass);
            await context.SubjectClasses.AddAsync(testSubject);
            await context.SaveChangesAsync();

            var actualResult = await this.gradeService.GetGradesForSubject("TestStudentWithNoGrades", "TestSubjectWithNoGrades").ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetGradesForSubject_WithExistentSubjectIdAndStudentIdAndDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "GradeService GetGradesForSubject() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var studentId = "TestStudent";
            var subjectId = "TestSubject";

            var actualResult = await this.gradeService.GetGradesForSubject(studentId, subjectId).ToListAsync();
            var expectedResult = new List<GradeServiceModel>();

            var student = context.Students.Find(studentId);
            var grades = student.Grades.Where(x => x.SubjectId == studentId);

            foreach (var grade in grades)
            {
                var model = grade.To<GradeServiceModel>();

                expectedResult.Add(model);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.GradeValue == expectedEntity.GradeValue, errorMessagePrefix + " " + "Grade Value is not returned properly.");
                Assert.True(actualEntity.Term == expectedEntity.Term, errorMessagePrefix + " " + "Term is not returned properly.");
                Assert.True(actualEntity.IsTermGrade == expectedEntity.IsTermGrade, errorMessagePrefix + " " + "IsTermGrade is not returned properly.");
            }
        }

        [Fact]
        public async Task GetGradesForSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "GradeService GetGradesForSubject() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var studentId = "Non_Existent";
            var subjectId = "Non_Existent";

            Assert.Throws<ArgumentNullException>(() => this.gradeService.GetGradesForSubject(studentId, subjectId));
        }

        [Fact]
        public async Task GetGrade_WithExistentId_ShouldSuccessfullyReturnGrade()
        {
            string errorMessagePrefix = "GradeService GetGradeById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = context.Grades.First().Id;

            var actualResult = await this.gradeService.GetGradeByIdAsync(testId);
            var expectedResult = context.Grades.First().To<GradeServiceModel>();

            Assert.True(actualResult.GradeValue == expectedResult.GradeValue, errorMessagePrefix + " " + "Grade Value is not returned properly.");
            Assert.True(actualResult.Term == expectedResult.Term, errorMessagePrefix + " " + "Term is not returned properly.");
            Assert.True(actualResult.IsTermGrade == expectedResult.IsTermGrade, errorMessagePrefix + " " + "IsTermGrade is not returned properly.");
        }

        [Fact]
        public async Task GetGrade_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "GradeService GetGradeById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.gradeService.GetGradeByIdAsync(testId));
        }

        [Fact]
        public async Task CreateGrade_WithCorrectData_ShouldSuccessfullyCreateGrade()
        {
            string errorMessagePrefix = "GradeService CreateGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var student = context.Students.First();

            var testGrade = new GradeServiceModel
            {
                Id = "TestCreateGrade",
                GradeValue = "5",
                Term = Term.Втори,
                IsTermGrade = false,
                Student = student.To<StudentServiceModel>(),
                StudentId = student.Id,
            };

            var actualResult = await this.gradeService.CreateGradeAsync(testGrade);
            var grade = context.Grades.Find("TestCreateGrade");

            Assert.True(actualResult, errorMessagePrefix);
            Assert.True(grade.GradeValue == testGrade.GradeValue, errorMessagePrefix + " " + "GradeValue is not set properly.");
            Assert.True(grade.Term == testGrade.Term, errorMessagePrefix + " " + "Term is not set properly.");
            Assert.True(grade.IsTermGrade == testGrade.IsTermGrade, errorMessagePrefix + " " + "IsTermGrade is not set properly.");
            Assert.True(grade.StudentId == testGrade.StudentId, errorMessagePrefix + " " + "StudentId is not set properly");
        }

        [Fact]
        public async Task CreateGrade_WithNonExistentStudent_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "GradeService CreateGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testGrade = new GradeServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.gradeService.CreateGradeAsync(testGrade));
        }

        [Fact]
        public async Task CreateGrade_WithAnotherTermGrade_ShouldReturnFalse()
        {
            string errorMessagePrefix = "GradeService CreateGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var studentId = "TestStudent";
            var subjectId = "TestSubject";

            var testGrade = new GradeServiceModel
            {
                GradeValue = "5",
                Term = Term.Първи,
                IsTermGrade = true,
                StudentId = studentId,
                SubjectId = subjectId,
            };

            var actualResult = await this.gradeService.CreateGradeAsync(testGrade);

            Assert.True(!actualResult, errorMessagePrefix + " " + "Another term grade is added");
        }

        [Fact]
        public async Task EditGrade_WithExistentId_ShouldSuccessfullyEditGrade()
        {
            string errorMessagePrefix = "GradeService EditGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = context.Grades.First().Id;

            var editGrade = new GradeServiceModel
            {
                Id = testId,
                GradeValue = "3",
                Term = Term.Втори,
                IsTermGrade = false,
            };

            var actualResult = await this.gradeService.EditGradeAsync(editGrade);

            Assert.True(actualResult, errorMessagePrefix);

            var editedGrade = context.Grades.First();

            Assert.True(editedGrade.GradeValue == editGrade.GradeValue, errorMessagePrefix + " " + "GradeValue is not set properly.");
            Assert.True(editedGrade.Term == editGrade.Term, errorMessagePrefix + " " + "Term is not set properly.");
            Assert.True(editedGrade.IsTermGrade == editGrade.IsTermGrade, errorMessagePrefix + " " + "IsTermGrade is not set properly.");
        }

        [Fact]
        public async Task EditGrade_WithExistentIdAndWithAnotherTermGrade_ShouldReturnFalse()
        {
            string errorMessagePrefix = "GradeService EditGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = context.Grades.First().Id;

            var editGrade = new GradeServiceModel
            {
                Id = testId,
                GradeValue = "3",
                Term = Term.Първи,
                IsTermGrade = true,
            };

            var actualResult = await this.gradeService.EditGradeAsync(editGrade);

            Assert.True(!actualResult, errorMessagePrefix + " " + "Can't change IsTermGrade to true when their is another Term Grade.");
        }

        [Fact]
        public async Task EditGrade_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "GradeService EditGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = "Non_Existent";

            var editGrade = new GradeServiceModel { Id = testId };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.gradeService.EditGradeAsync(editGrade));
        }

        [Fact]
        public async Task DeleteGrade_WithExistentId_ShouldSuccessfullyDeleteGrade()
        {
            string errorMessagePrefix = "GradeService DeleteGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = context.Grades.First().Id;

            var actualResult = await this.gradeService.DeleteGradeAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var deletedGrade = context.Grades.Find(testId);

            Assert.True(deletedGrade.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly");
        }

        [Fact]
        public async Task DeleteGrade_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "GradeService DeleteGradeAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.gradeService = new GradeService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.gradeService.DeleteGradeAsync(testId));
        }
    }
}
