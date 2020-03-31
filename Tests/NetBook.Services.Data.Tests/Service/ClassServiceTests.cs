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
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class ClassServiceTests
    {
        private IClassService classService;

        public ClassServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Class> GetDummyData()
        {
            return new List<Class>
            {
                new Class
                {
                    Id = "WithSubjectClasses",
                    ClassNumber = 12,
                    ClassLetter = ClassLetter.A,
                    ClassTeacher = new NetBookUser { FullName = "TestTeacher" },
                    SchoolYearStart = 2018,
                    SchoolYearEnd = 2019,
                    Subjects = new List<SubjectClass>
                    {
                        new SubjectClass
                        {
                            Id = "TestId",
                            Subject = new Subject { Name = "TestSubject" },
                            Workload = 150,
                        },
                        new SubjectClass
                        {
                            Subject = new Subject { Name = "TestSubject2" },
                            Workload = 225,
                        },
                        new SubjectClass
                        {
                            Subject = new Subject { Name = "TestSubject3" },
                            Workload = 75,
                        },
                    },
                    Students = new List<Student>
                    {
                      new Student
                      {
                          Id = "WithAbsences",
                          FullName = "TestStudent",
                          Absences = new List<Absence>
                          {
                              new Absence { StudentId = "WithAbsences", SubjectId = "TestId" },
                              new Absence { StudentId = "WithAbsences", SubjectId = "TestId" },
                          },
                          Remarks = new List<Remark>
                          {
                              new Remark { StudentId = "WithAbsences", SubjectId = "TestId", Text = "TestRemark" },
                              new Remark { StudentId = "WithAbsences", SubjectId = "TestId", Text = "TestRemark2" },
                          },
                      },
                      new Student
                      {
                          Id = "NoAbsences",
                          FullName = "TestStudent2",
                      },
                    },
                },
                new Class
                {
                    Id = "NoSubjectClasses",
                    ClassNumber = 11,
                    ClassLetter = ClassLetter.Б,
                    ClassTeacher = new NetBookUser { FullName = "TestTeacher2" },
                    SchoolYearStart = 2017,
                    SchoolYearEnd = 2018,
                },
                new Class
                {
                    Id = "NoStudents",
                    ClassNumber = 10,
                    ClassLetter = ClassLetter.В,
                    ClassTeacher = new NetBookUser { FullName = "TestTeacher3" },
                    SchoolYearStart = 2019,
                    SchoolYearEnd = 2020,
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllClasses_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllClasses() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.classService = new ClassService(context);

            List<ClassServiceModel> actualResult = await this.classService.GetAllClasses().ToListAsync();
            int expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllClasses_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetAllClasses() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            List<ClassServiceModel> actualResult = await this.classService.GetAllClasses().ToListAsync();
            List<ClassServiceModel> expectedResult = await context.Classes
                .OrderBy(x => x.ClassNumber)
                .ThenBy(x => x.ClassLetter)
                .To<ClassServiceModel>().ToListAsync();

            Assert.Equal(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntity = expectedResult[i];
                var actualEntity = actualResult[i];

                Assert.True($"{expectedEntity.ClassNumber} {expectedEntity.ClassLetter}" == $"{actualEntity.ClassNumber} {actualEntity.ClassLetter}", errorMessagePrefix + " " + "Class Name is not returned properly");
                Assert.True($"{expectedEntity.SchoolYearStart} {expectedEntity.SchoolYearEnd}" == $"{actualEntity.SchoolYearStart} {actualEntity.SchoolYearEnd}", errorMessagePrefix + " " + "Class Year is not returned properly");
                Assert.True(expectedEntity.ClassTeacher.FullName == actualEntity.ClassTeacher.FullName, errorMessagePrefix + " " + "ClassTeacher Full Name is not returned properly");
            }
        }

        [Fact]
        public async Task GetClass_WithExistentId_ShouldSuccessfullyReturnClass()
        {
            string errorMessagePrefix = "ClassService GetClassById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            string testId = context.Classes.First().Id;

            var actualResult = this.classService.GetClassById(testId);
            var expectedResult = context.Classes.First();

            Assert.True($"{expectedResult.ClassNumber} {expectedResult.ClassLetter}" == $"{actualResult.ClassNumber} {actualResult.ClassLetter}", errorMessagePrefix + " " + "Class Name is not returned properly");
            Assert.True($"{expectedResult.SchoolYearStart} {expectedResult.SchoolYearEnd}" == $"{actualResult.SchoolYearStart} {actualResult.SchoolYearEnd}", errorMessagePrefix + " " + "Class Year is not returned properly");
            Assert.True(expectedResult.ClassTeacher.FullName == actualResult.ClassTeacher.FullName, errorMessagePrefix + " " + "ClassTeacher Full Name is not returned properly");
        }

        [Fact]
        public async Task GetClass_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetClassById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            string testId = "Non_Existent";

            Assert.Throws<ArgumentNullException>(() => this.classService.GetClassById(testId));
        }

        [Fact]
        public async Task GetClassName_WithExistentId_ShouldSuccessfullyReturnClassName()
        {
            string errorMessagePrefix = "ClassService GetClassNameById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            string testId = context.Classes.First().Id;
            var classEntity = context.Classes.First();

            var actualResult = await this.classService.GetClassNameByIdAsync(testId);
            var expectedResult = $"{classEntity.ClassNumber} {classEntity.ClassLetter}";

            Assert.True(expectedResult == actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetClassName_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetClassNameById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.GetClassNameByIdAsync(testId));
        }

        [Fact]
        public async Task GetStudentClass_WithExistentId_ShouldSuccessfullyReturnClass()
        {
            string errorMessagePrefix = "ClassService GetStudentClassDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var classEntity = context.Classes.First();

            var testId = classEntity.Students.First().Id;

            var actualResult = await this.classService.GetStudentClassDropdownAsync(testId);

            var expectedResult = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = $"{classEntity.ClassNumber} {classEntity.ClassLetter}",
                    Value = classEntity.Id,
                },
            };

            Assert.Equal(expectedResult.Count, actualResult.Count);

            Assert.True(actualResult.First().Text == expectedResult.First().Text, errorMessagePrefix + " " + "ClassName is not returned properly.");
            Assert.True(actualResult.First().Value == expectedResult.First().Value, errorMessagePrefix + " " + "ClassId is not returned properly.");
        }

        [Fact]
        public async Task GetStudentClass_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetStudentClassDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.GetStudentClassDropdownAsync(testId));
        }

        [Fact]
        public async Task GetClassesDropdown_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetClassesDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.classService = new ClassService(context);

            var actualResult = await this.classService.GetClassesDropdownAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetClassesDropdown_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetClassesDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var actualResult = await this.classService.GetClassesDropdownAsync();
            var expectedResult = new List<SelectListItem>();

            var classes = await context.Classes.ToListAsync();

            foreach (var classEntity in classes)
            {
                var item = new SelectListItem
                {
                    Text = $"{classEntity.ClassNumber} {classEntity.ClassLetter}",
                    Value = classEntity.Id,
                };

                expectedResult.Add(item);
            }

            Assert.Equal(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntry = expectedResult[i];
                var actualEntry = actualResult[i];

                Assert.True(expectedEntry.Text == actualEntry.Text, errorMessagePrefix + " " + "ClassName is not returned properly.");
                Assert.True(expectedEntry.Value == actualEntry.Value, errorMessagePrefix + " " + "ClassId is not returned properly.");
            }
        }

        [Fact]
        public async Task GetClassNumbersDropdown_ShouldReturnCorrectData()
        {
            string errorMessagePrefix = "ClassService GetClassNumbersDropdown() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var actualResult = this.classService.GetClassNumbersDropdown();
            var expectedResult = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "8",
                    Value = "8",
                },
                new SelectListItem
                {
                    Text = "9",
                    Value = "9",
                },
                new SelectListItem
                {
                    Text = "10",
                    Value = "10",
                },
                new SelectListItem
                {
                    Text = "11",
                    Value = "11",
                },
                new SelectListItem
                {
                    Text = "12",
                    Value = "12",
                },
            };

            Assert.Equal(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntry = expectedResult[i];
                var actualEntry = actualResult[i];

                Assert.True(expectedEntry.Text == actualEntry.Text, errorMessagePrefix + " " + "ClassNumber is not returned properly.");
                Assert.True(expectedEntry.Value == actualEntry.Value, errorMessagePrefix + " " + "ClassNumber Value is not returned properly.");
            }
        }

        [Fact]
        public async Task CreateClass_WithNonExistentTeacher_ShouldThrowArgumentNullException()
        {
            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testClass = new ClassServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.CreateClassAsync(testClass));
        }

        [Fact]
        public async Task CreateClass_WithCorrectData_ShouldSuccessfullyCreateClass()
        {
            string errorMessagePrefix = "ClassService CreateClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testTeacher = new NetBookUser
            {
                Id = "test",
                FullName = "TestTeacher",
                IsTeacher = false,
            };

            await context.Users.AddAsync(testTeacher);
            await context.SaveChangesAsync();

            var testClass = new ClassServiceModel
            {
                Id = "testClass",
                ClassTeacherId = testTeacher.Id,
                ClassTeacher = testTeacher.To<UserServiceModel>(),
            };

            var actualResult = await this.classService.CreateClassAsync(testClass);
            var updatedTeacher = context.Users.Find("test");
            var classId = updatedTeacher.ClassId;
            var isClassTeacher = updatedTeacher.IsClassTeacher;

            Assert.True(actualResult, errorMessagePrefix);
            Assert.True(isClassTeacher, errorMessagePrefix + " " + "IsClassTeacher is not set properly");
            Assert.True(classId == "testClass", errorMessagePrefix + " " + "ClassId is not set properly to Teacher.");
        }

        [Fact]
        public async Task EditClass_WithIncorrectData_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService EditClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            var testClassModel = new ClassServiceModel { Id = testId };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.EditClassAsync(testClassModel));
        }

        [Fact]
        public async Task EditClass_WithCorrectData_ShouldSuccessfullyEditClass()
        {
            string errorMessagePrefix = "ClassService EditClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = context.Classes.First().Id;

            var testEditClass = new ClassServiceModel
            {
                Id = testId,
                ClassNumber = 10,
                ClassLetter = ClassLetter.Б,
                SchoolYearStart = 2017,
                SchoolYearEnd = 2018,
            };

            var actualResult = await this.classService.EditClassAsync(testEditClass);

            var editedClass = context.Classes.First();

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(testEditClass.ClassNumber == editedClass.ClassNumber, errorMessagePrefix + " " + "ClassNumber is not set properly.");
            Assert.True(testEditClass.ClassLetter == editedClass.ClassLetter, errorMessagePrefix + " " + "ClassLetter is not set properly.");
            Assert.True(testEditClass.SchoolYearStart == editedClass.SchoolYearStart, errorMessagePrefix + " " + "SchoolYearStart is not set properly.");
            Assert.True(testEditClass.SchoolYearEnd == editedClass.SchoolYearEnd, errorMessagePrefix + " " + "SchoolYearEnd is not set properly.");
        }

        [Fact]
        public async Task EditClass_WithSchoolYearDifferenceBiggerOrLessThan1_ShouldReturnFalse()
        {
            string errorMessagePrefix = "ClassService EditClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = context.Classes.First().Id;

            var testEditClassLess = new ClassServiceModel
            {
                Id = testId,
                ClassNumber = 10,
                ClassLetter = ClassLetter.Б,
                SchoolYearStart = 2017,
                SchoolYearEnd = 2017,
            };

            var testEditClassBigger = new ClassServiceModel
            {
                Id = testId,
                ClassNumber = 10,
                ClassLetter = ClassLetter.Б,
                SchoolYearStart = 2017,
                SchoolYearEnd = 2019,
            };

            var actualResultLess = await this.classService.EditClassAsync(testEditClassLess);
            var actualResultBigger = await this.classService.EditClassAsync(testEditClassBigger);

            Assert.True(!actualResultLess, errorMessagePrefix + " " + "SchoolYearStart and SchoolYearEnd difference not checked properly.");
            Assert.True(!actualResultBigger, errorMessagePrefix + " " + "SchoolYearStart and SchoolYearEnd difference not checked properly.");
        }

        [Fact]
        public async Task DeleteClass_WithExistentId_ShouldSuccessfullyDeleteClass()
        {
            string errorMessagePrefix = "ClassService DeleteClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = context.Classes.First().Id;

            var actualResult = await this.classService.DeleteClassAsync(testId);

            var deletedClass = context.Classes.Find(testId);
            var students = deletedClass.Students;
            var subjects = deletedClass.Subjects;
            var teacher = deletedClass.ClassTeacher;

            Assert.True(actualResult, errorMessagePrefix);
            Assert.True(deletedClass.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly.");
            Assert.True(teacher.IsDeleted, errorMessagePrefix + " " + "Teacher IsDeleted is not set properly.");

            foreach (var student in students)
            {
                Assert.True(student.IsDeleted, errorMessagePrefix + " " + "Student IsDeleted is not set properly.");
            }

            foreach (var subject in subjects)
            {
                Assert.True(subject.IsDeleted, errorMessagePrefix + " " + "Subject IsDeleted is not set properly.");
            }
        }

        [Fact]
        public async Task DeleteClass_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService DeleteClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.DeleteClassAsync(testId));
        }

        [Fact]
        public async Task GetAllSubjectClasses_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllSubjectClasses() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "NoSubjectClasses";

            var actualResult = await this.classService.GetAllSubjectClasses(testId).ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllSubjectClasses_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetAllSubjectClasses() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "WithSubjectClasses";

            var actualResult = await this.classService.GetAllSubjectClasses(testId).ToListAsync();
            var expectedResult = new List<ClassSubjectServiceModel>();

            var classEntity = context.Classes.Find(testId);
            var subjects = classEntity.Subjects;

            foreach (var subject in subjects)
            {
                var model = subject.To<ClassSubjectServiceModel>();

                expectedResult.Add(model);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(expectedEntity.Subject.Name == actualEntity.Subject.Name, errorMessagePrefix + " " + "SubjectName is not returned properly.");
                Assert.True(expectedEntity.Workload == actualEntity.Workload, errorMessagePrefix + " " + "Workload is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllSubjectsInClassDropdown_WithZeroDataAndExistentId_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetAllSubjectsInClassDropdown() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "NoSubjectClasses";

            var actualResult = await this.classService.GetAllSubjectsInClassDropdownAsync(testId);
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllSubjectsInClassDropdown_WithDummyDataAndExistentId_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetAllSubjectsInClassDropdown() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "WithSubjectClasses";

            var actualResult = await this.classService.GetAllSubjectsInClassDropdownAsync(testId);
            var expectedResult = new List<SelectListItem>();

            var classEntity = context.Classes.Find(testId);

            var subjects = classEntity.Subjects;

            foreach (var subject in subjects)
            {
                var item = new SelectListItem
                {
                    Text = subject.Subject.Name,
                    Value = subject.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntity = expectedResult[i];
                var actualEntity = actualResult[i];

                Assert.True(expectedEntity.Text == actualEntity.Text, errorMessagePrefix + " " + "Subject Name is not returned properly.");
                Assert.True(expectedEntity.Value == actualEntity.Value, errorMessagePrefix + " " + "Subject Id is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllSubjectsInClassDropdown_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetAllSubjectsInClassDropdown() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.GetAllSubjectsInClassDropdownAsync(testId));
        }

        [Fact]
        public async Task GetSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetSubjectById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.GetSubjectById(testId));
        }

        [Fact]
        public async Task GetSubject_WithExistentId_ShouldReturnSubject()
        {
            string errorMessagePrefix = "ClassService GetSubjectById() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "TestId";

            var actualResult = await this.classService.GetSubjectById(testId);
            var expectedResult = context.SubjectClasses.Find(testId).To<ClassSubjectServiceModel>();

            Assert.True(expectedResult.Subject.Name == actualResult.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
            Assert.True(expectedResult.Workload == actualResult.Workload, errorMessagePrefix + " " + "Workload is not returned properly.");
        }

        [Fact]
        public async Task CreateSubject_WithCorrectData_ShouldSuccessfullyCreateSubject()
        {
            string errorMessagePrefix = "ClassService CreateSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var classId = "NoSubjectClasses";

            var testSubject = new ClassSubjectServiceModel
            {
                Id = "TestCreateSubject",
                Subject = new SubjectServiceModel { Name = "TestSubject" },
                Workload = 175,
                ClassId = classId,
            };

            var actualResult = await this.classService.CreateSubjectAsync(testSubject);

            Assert.True(actualResult, errorMessagePrefix);

            var subject = context.SubjectClasses.Find("TestCreateSubject");

            Assert.True(subject.Subject.Name == testSubject.Subject.Name, errorMessagePrefix + " " + "Subject Name is not set properly.");
            Assert.True(subject.Workload == testSubject.Workload, errorMessagePrefix + " " + "Workload is not set properly.");
            Assert.True(subject.ClassId == testSubject.ClassId, errorMessagePrefix + " " + "ClassId is not set properly");
        }

        [Fact]
        public async Task CreateSubject_WithIncorrectData_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService CreateSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testSubject = new ClassSubjectServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.CreateSubjectAsync(testSubject));
        }

        [Fact]
        public async Task EditSubject_WithIncorrectData_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService EditSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testSubject = new ClassSubjectServiceModel { Id = "Non_Existent" };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.EditSubjectAsync(testSubject));
        }

        [Fact]
        public async Task EditSubject_WithCorrectData_ShouldSuccessfullyEditSubject()
        {
            string errorMessagePrefix = "ClassService EditSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var subject = context.SubjectClasses.First();

            var testSubject = subject.To<ClassSubjectServiceModel>();
            testSubject.Workload = 323;

            var actualResult = await this.classService.EditSubjectAsync(testSubject);

            Assert.True(actualResult, errorMessagePrefix);

            var expectedResult = context.SubjectClasses.First();

            Assert.True(expectedResult.Workload == testSubject.Workload, errorMessagePrefix + " " + "Workload is not set properly.");
        }

        [Fact]
        public async Task DeleteSubject_WithExistentId_ShouldSuccessfullyDeleteSubject()
        {
            string errorMessagePrefix = "ClassService DeleteSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "TestId";

            var actualResult = await this.classService.DeleteSubjectAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var deletedSubject = context.SubjectClasses.Find(testId);

            Assert.True(deletedSubject.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly");
        }

        [Fact]
        public async Task DeleteSubject_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService DeleteSubjectAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.DeleteSubjectAsync(testId));
        }

        [Fact]
        public async Task GetAllStudentsInClassDropdown_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllStudentsInClassDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "NoStudents";

            var actualResult = await this.classService.GetAllStudentsInClassDropdownAsync(testId);
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllStudentsInClassDropdown_WithDummyData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllStudentsInClassDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = context.Classes.First().Id;

            var actualResult = await this.classService.GetAllStudentsInClassDropdownAsync(testId);
            var expectedResult = new List<SelectListItem>();

            var classEntity = context.Classes.First();

            var students = classEntity.Students;

            foreach (var student in students)
            {
                var item = new SelectListItem
                {
                    Text = student.FullName,
                    Value = student.Id,
                };

                expectedResult.Add(item);
            }

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(expectedEntity.Text == actualEntity.Text, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
                Assert.True(expectedEntity.Value == actualEntity.Value, errorMessagePrefix + " " + "Student Id is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllStudentsInClassDropdown_WithNonExistentClassId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetAllStudentsInClassDropdownAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.classService.GetAllStudentsInClassDropdownAsync(testId));
        }

        [Fact]
        public async Task GetAllAbsences_WithExistentIdAndZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllAbsences() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "NoAbsences";

            var actualResult = await this.classService.GetAllAbsences(testId).ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllAbsences_WithExistentIdAndDummyData_ShouldReturnAllAbsences()
        {
            string errorMessagePrefix = "ClassService GetAllAbsences() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "WithAbsences";

            var actualResult = await this.classService.GetAllAbsences(testId).ToListAsync();
            var expectedResult = context.Students.Find(testId).Absences.To<AbsenceServiceModel>().ToList();

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(expectedEntity.SubjectId == actualEntity.SubjectId, errorMessagePrefix + " " + "SubjectId not returned properly");
                Assert.True(expectedEntity.StudentId == actualEntity.StudentId, errorMessagePrefix + " " + "StudentId not returned properly");
            }
        }

        [Fact]
        public async Task GetAllAbsences_WithNonExistentStudentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetAllAbsences() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            Assert.Throws<ArgumentNullException>(() => this.classService.GetAllAbsences(testId));
        }

        [Fact]
        public async Task GetAllRemarks_WithExistentIdAndZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ClassService GetAllRemarks() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "NoAbsences";

            var actualResult = await this.classService.GetAllRemarks(testId).ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllRemarks_WithExistentIdAndDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ClassService GetAllRemarks() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "WithAbsences";

            var actualResult = await this.classService.GetAllRemarks(testId).ToListAsync();
            var expectedResult = context.Students.Find(testId).Remarks.OrderBy(x => x.Subject.Subject.Name).ThenBy(x => x.Text).To<RemarkServiceModel>().ToList();

            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(expectedEntity.StudentId == actualEntity.StudentId, errorMessagePrefix + " " + "StudentId is not set properly.");
                Assert.True(expectedEntity.SubjectId == actualEntity.SubjectId, errorMessagePrefix + " " + "SubjectId is not set properly.");
                Assert.True(expectedEntity.Text == actualEntity.Text, errorMessagePrefix + " " + "Text is not set properly.");
            }
        }

        [Fact]
        public async Task GetAllRemarks_WithNonExistentStudentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ClassService GetAllRemarks() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.classService = new ClassService(context);

            var testId = "Non_Existent";

            Assert.Throws<ArgumentNullException>(() => this.classService.GetAllRemarks(testId));
        }
    }
}
