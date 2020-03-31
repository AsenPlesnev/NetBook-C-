namespace NetBook.Services.Data.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class StudentServiceTests
    {
        private IStudentService studentService;

        public StudentServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Student> GetDummyData()
        {
            return new List<Student>
            {
                new Student
                {
                    FullName = "TestStudent",
                    PIN = "TestPin",
                    Class = new Class
                    {
                        ClassNumber = 10,
                        ClassLetter = ClassLetter.Б,
                        ClassTeacher = new NetBookUser { FullName = "Test" },
                        Subjects = new List<SubjectClass>
                        {
                            new SubjectClass
                            {
                                Id = "TestSubjectClass",
                                Subject = new Subject
                                {
                                    Id = "TestSubject",
                                    Name = "TestSubject",
                                },
                            },
                        },
                    },
                    DateOfBirth = DateTime.UtcNow,
                    Grades = new List<Grade>
                    {
                        new Grade
                        {
                            GradeValue = "5",
                            Term = Term.Втори,
                            IsTermGrade = true,
                            SubjectId = "TestSubjectClass",
                        },
                        new Grade
                        {
                            GradeValue = "4",
                            Term = Term.Втори,
                            IsTermGrade = false,
                            SubjectId = "TestSubjectClass",
                        },
                        new Grade
                        {
                            GradeValue = "5",
                            Term = Term.Първи,
                            IsTermGrade = true,
                            SubjectId = "TestSubjectClass",
                        },
                        new Grade
                        {
                            GradeValue = "4",
                            Term = Term.Първи,
                            IsTermGrade = false,
                            SubjectId = "TestSubjectClass",
                        },
                    },
                    Absences = new List<Absence>
                    {
                        new Absence(),
                    },
                    Remarks = new List<Remark>
                    {
                        new Remark
                        {
                            Text = "Test",
                        },
                    },
                },
                new Student
                {
                    Id = "WithSubjects",
                    FullName = "TestStudent2",
                    PIN = "TestPin2",
                    Class = new Class
                    {
                        ClassNumber = 11,
                        ClassLetter = ClassLetter.A,
                        ClassTeacher = new NetBookUser { FullName = "Test" },
                        Subjects = new List<SubjectClass>
                        {
                            new SubjectClass
                            {
                                Subject = new Subject { Name = "TestSubject" },
                                Workload = 125,
                            },
                            new SubjectClass
                            {
                                Subject = new Subject { Name = "TestSubject2" },
                                Workload = 120,
                            },
                        },
                    },
                    Grades = new List<Grade>
                    {
                        new Grade
                        {
                            GradeValue = "4",
                            Term = Term.Втори,
                            IsTermGrade = false,
                            SubjectId = "TestSubjectClass",
                        },
                        new Grade
                        {
                            GradeValue = "4",
                            Term = Term.Първи,
                            IsTermGrade = false,
                            SubjectId = "TestSubjectClass",
                        },
                    },
                    Remarks = new List<Remark>(),
                    Absences = new List<Absence>(),
                },
                new Student
                {
                    Id = "NoSubjects",
                    FullName = "TestStudent3",
                    PIN = "TestPin3",
                    Class = new Class
                    {
                        ClassNumber = 12,
                        ClassLetter = ClassLetter.В,
                    },
                },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllStudents_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "StudentService GetAllStudents() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.studentService = new StudentService(context);

            var actualResult = await this.studentService.GetAllStudents().ToListAsync();
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllStudents_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "StudentService GetAllStudents() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var actualResult = await this.studentService.GetAllStudents().ToListAsync();
            var expectedResult = await context.Students.OrderBy(x => x.Class.ClassNumber)
                .ThenBy(x => x.Class.ClassLetter)
                .ThenBy(x => x.FullName)
                .To<StudentServiceModel>()
                .ToListAsync();

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.FullName == expectedEntity.FullName, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
                Assert.True(actualEntity.PIN == expectedEntity.PIN, errorMessagePrefix + " " + "Student PIN is not returned properly.");
                Assert.True($"{actualEntity.Class.ClassNumber} {actualEntity.Class.ClassLetter}" == $"{expectedEntity.Class.ClassNumber} {expectedEntity.Class.ClassLetter}", errorMessagePrefix + " " + "Student Class Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetStudentSubjects_WithExistentIdAndEmptyClass_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "StudentService GetStudentSubjectsAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "NoSubjects";

            var actualResult = await this.studentService.GetStudentSubjectsAsync(testId);
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetStudentSubjects_WithExistentIdAndNonEmptyClass_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "StudentService GetStudentSubjectsAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "WithSubjects";

            var actualResult = await this.studentService.GetStudentSubjectsAsync(testId);
            var expectedResult = new List<ClassSubjectServiceModel>();

            var subjects = context.Students.Find(testId).Class.Subjects.OrderBy(x => x.Subject.Name).To<ClassSubjectServiceModel>().ToList();

            expectedResult = subjects;

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.Subject.Name == expectedEntity.Subject.Name, errorMessagePrefix + " " + "Subject Name is not returned properly.");
                Assert.True(actualEntity.Workload == expectedEntity.Workload, errorMessagePrefix + " " + "Subject Workload is not returned properly.");
            }
        }

        [Fact]
        public async Task GetStudentSubjects_WithNonExistentStudentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService GetStudentSubjectsAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.GetStudentSubjectsAsync(testId));
        }

        [Fact]
        public async Task GetStudent_WithExistentId_ShouldSuccessfullyReturnStudent()
        {
            string errorMessagePrefix = "StudentService GetStudentByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = context.Students.First().Id;

            var actualResult = await this.studentService.GetStudentByIdAsync(testId);
            var expectedResult = context.Students.First().To<StudentServiceModel>();

            Assert.True(actualResult.FullName == expectedResult.FullName, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
            Assert.True(actualResult.PIN == expectedResult.PIN, errorMessagePrefix + " " + "Student PIN is not returned properly.");
            Assert.True($"{actualResult.Class.ClassNumber} {actualResult.Class.ClassLetter}" == $"{expectedResult.Class.ClassNumber} {expectedResult.Class.ClassLetter}", errorMessagePrefix + " " + "Student Class Name is not returned properly.");
        }

        [Fact]
        public async Task GetStudent_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService GetStudentByIdAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.GetStudentByIdAsync(testId));
        }

        [Fact]
        public async Task GetAllStudentsInClass_WithExistentIdAndZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "StudentService GetAllStudentsInClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.studentService = new StudentService(context);

            var testClass = new Class { Id = "TestClass" };

            await context.Classes.AddAsync(testClass);
            await context.SaveChangesAsync();

            var actualResult = await this.studentService.GetAllStudentsInClassAsync(testClass.Id);
            var expectedResult = 0;

            Assert.True(actualResult.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllStudentsInClass_WithExistentIdAndDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "StudentService GetAllStudentsInClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = context.Classes.First().Id;

            var actualResult = await this.studentService.GetAllStudentsInClassAsync(testId);
            var expectedResult = await context.Students.Where(x => x.ClassId == testId).To<StudentServiceModel>().ToListAsync();

            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var actualEntity = actualResult[i];
                var expectedEntity = expectedResult[i];

                Assert.True(actualEntity.FullName == expectedEntity.FullName, errorMessagePrefix + " " + "Student Full Name is not returned properly.");
                Assert.True(actualEntity.PIN == expectedEntity.PIN, errorMessagePrefix + " " + "Student PIN is not returned properly.");
                Assert.True($"{actualEntity.Class.ClassNumber} {actualEntity.Class.ClassLetter}" == $"{expectedEntity.Class.ClassNumber} {expectedEntity.Class.ClassLetter}", errorMessagePrefix + " " + "Student Class Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllStudentsInClass_WithNonExistentClassId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService GetAllStudentsInClassAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.GetAllStudentsInClassAsync(testId));
        }

        [Fact]
        public async Task CreateStudent_WithCorrectData_ShouldSuccessfullyCreateStudent()
        {
            string errorMessagePrefix = "StudentService CreateStudentAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var classId = context.Classes.First().Id;

            var testStudent = new StudentServiceModel
            {
                Id = "CreateStudent",
                ClassId = classId,
            };

            var actualResult = await this.studentService.CreateStudentAsync(testStudent);

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(context.Students.Any(x => x.Id == "CreateStudent"), errorMessagePrefix + " " + "Student is not added to context.");
        }

        [Fact]
        public async Task CreateStudent_WithNonExistentClassId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService CreateStudentAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testStudent = new StudentServiceModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.CreateStudentAsync(testStudent));
        }

        [Fact]
        public async Task EditStudent_WithExistentId_ShouldSuccessfullyEditStudent()
        {
            string errorMessagePrefix = "StudentService EditStudentAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = context.Students.First().Id;

            var testStudent = new StudentServiceModel
            {
                Id = testId,
                FullName = "EditStudent",
                PIN = "EditPIN",
                Citizenship = "TestCitizenship",
                DateOfBirth = DateTime.UtcNow,
                PhoneNumber = "TestPhone",
                Town = "TestTown",
                Municipality = "TestMunicipality",
                Region = "TestRegion",
                Address = "TestAddress",
            };

            var actualResult = await this.studentService.EditStudentAsync(testStudent);
            var editedStudent = context.Students.Find(testId);

            Assert.True(actualResult, errorMessagePrefix);

            Assert.True(editedStudent.FullName == testStudent.FullName, errorMessagePrefix + " " + "FullName is not set properly");
            Assert.True(editedStudent.PIN == testStudent.PIN, errorMessagePrefix + " " + "PIN is not set properly");
            Assert.True(editedStudent.Citizenship == testStudent.Citizenship, errorMessagePrefix + " " + "Citizenship is not set properly");
            Assert.True(editedStudent.DateOfBirth == testStudent.DateOfBirth, errorMessagePrefix + " " + "DateOfBirth is not set properly");
            Assert.True(editedStudent.PhoneNumber == testStudent.PhoneNumber, errorMessagePrefix + " " + "PhoneNumber is not set properly");
            Assert.True(editedStudent.Town == testStudent.Town, errorMessagePrefix + " " + "Town is not set properly");
            Assert.True(editedStudent.Municipality == testStudent.Municipality, errorMessagePrefix + " " + "Municipality is not set properly");
            Assert.True(editedStudent.Region == testStudent.Region, errorMessagePrefix + " " + "Region is not set properly");
            Assert.True(editedStudent.Address == testStudent.Address, errorMessagePrefix + " " + "Address is not set properly");
        }

        [Fact]
        public async Task EditStudent_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService EditStudentAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "Non_Existent";

            var testStudent = new StudentServiceModel { Id = testId };

            await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await this.studentService.EditStudentAsync(testStudent));
        }

        [Fact]
        public async Task DeleteStudent_WithExistentId_ShouldSuccessfullyDeleteStudent()
        {
            string errorMessagePrefix = "StudentService DeleteStudent() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = context.Students.First().Id;

            var actualResult = await this.studentService.DeleteStudentAsync(testId);

            Assert.True(actualResult, errorMessagePrefix);

            var deletedStudent = context.Students.Find(testId);

            Assert.True(deletedStudent.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set properly.");

            var grades = deletedStudent.Grades;
            var absences = deletedStudent.Absences;
            var remarks = deletedStudent.Remarks;

            foreach (var grade in grades)
            {
                Assert.True(grade.IsDeleted, errorMessagePrefix + " " + "Grade IsDeleted is not set properly.");
            }

            foreach (var absence in absences)
            {
                Assert.True(absence.IsDeleted, errorMessagePrefix + " " + "Absence IsDeleted is not set properly.");
            }

            foreach (var remark in remarks)
            {
                Assert.True(remark.IsDeleted, errorMessagePrefix + " " + "Remark IsDeleted is not set properly.");
            }
        }

        [Fact]
        public async Task DeleteStudent_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService DeleteStudent() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.DeleteStudentAsync(testId));
        }

        [Fact]
        public async Task GetStudentToDisplay_WithExistentStudentPinAndZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "StudentService GetStudentToDisplayAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            this.studentService = new StudentService(context);

            var student = new Student
            {
                PIN = "TestPIN",
                FullName = "TestName",
                Absences = new List<Absence>(),
                Grades = new List<Grade>(),
                Remarks = new List<Remark>(),
                Class = new Class { ClassTeacher = new NetBookUser() },
            };

            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            var actualResult = await this.studentService.GetStudentToDisplayAsync(student.PIN);
            var expectedResult = 0;

            Assert.True(actualResult.GradesForSubject.Count == expectedResult, errorMessagePrefix);
            Assert.True(actualResult.Absences.Count == expectedResult, errorMessagePrefix);
            Assert.True(actualResult.Remarks.Count == expectedResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetStudentToDisplay_WithExistentStudentPinAndDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "StudentService GetStudentToDisplayAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testPin = context.Students.First().PIN;

            var actualResult = await this.studentService.GetStudentToDisplayAsync(testPin);
            var student = context.Students.First(x => x.PIN == testPin);

            Assert.True(actualResult.GradesForSubject.Count == student.Class.Subjects.Count, errorMessagePrefix + " " + "Subject counts not returned properly");

            foreach (var gradesInSubject in actualResult.GradesForSubject)
            {
                var subjectId = gradesInSubject.Subject.Id;

                var firstTermGrades = student.Grades.Where(x => x.SubjectId == subjectId && x.Term == Term.Първи && !x.IsTermGrade).ToList();

                Assert.True(gradesInSubject.FirstTermGrades.Count == firstTermGrades.Count, errorMessagePrefix + " " + "FirstTermGrades count is not returned properly.");

                var secondTermGrades = student.Grades.Where(x => x.SubjectId == subjectId && x.Term == Term.Втори && !x.IsTermGrade).ToList();

                Assert.True(gradesInSubject.SecondTermGrades.Count == secondTermGrades.Count, errorMessagePrefix + " " + "SecondTermGrades count is not returned properly.");

                var firstTermGrade = student.Grades.First(x => x.SubjectId == subjectId && x.Term == Term.Първи && x.IsTermGrade);

                Assert.True(gradesInSubject.FirstTermGrade == firstTermGrade.GradeValue, errorMessagePrefix + " " + "FirstTermGrade not set properly.");

                var secondTermGrade = student.Grades.First(x => x.SubjectId == subjectId && x.Term == Term.Втори && x.IsTermGrade);

                Assert.True(gradesInSubject.SecondTermGrade == secondTermGrade.GradeValue, errorMessagePrefix + " " + "SecondTermGrade not set properly.");
            }

            Assert.True(actualResult.Absences.Count == student.Absences.Count, errorMessagePrefix + " " + "Absences are not returned properly.");

            Assert.True(actualResult.Remarks.Count == student.Remarks.Count, errorMessagePrefix + " " + "Remarks are not returned properly.");
        }

        [Fact]
        public async Task GetStudentToDisplay_WithExistentPinAndDummyDataAndNoTermGrades_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "StudentService GetStudentToDisplayAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var student = context.Students.Find("WithSubjects");

            var actualResult = await this.studentService.GetStudentToDisplayAsync(student.PIN);
            var expectedResult = "Няма";

            foreach (var gradesInSubject in actualResult.GradesForSubject)
            {
                Assert.True(gradesInSubject.FirstTermGrade == expectedResult, errorMessagePrefix + " " + "Null Term Grade is not set properly.");
                Assert.True(gradesInSubject.SecondTermGrade == expectedResult, errorMessagePrefix + " " + "Null Term Grade is not set properly.");
            }
        }

        [Fact]
        public async Task GetStudentToDisplay_WithNonExistentPin_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "StudentService GetStudentToDisplayAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.studentService = new StudentService(context);

            var testPin = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.studentService.GetStudentToDisplayAsync(testPin));
        }
    }
}
