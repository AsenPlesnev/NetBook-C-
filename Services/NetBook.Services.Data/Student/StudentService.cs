namespace NetBook.Services.Data.Student
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext context;

        public StudentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<StudentServiceModel> GetAllStudents()
        {
            return this.context.Students.Include(x => x.Class).Where(s => !s.IsDeleted)
                .OrderBy(x => x.Class.ClassNumber)
                .ThenBy(x => x.Class.ClassLetter)
                .ThenBy(x => x.FullName)
                .To<StudentServiceModel>();
        }

        public async Task<StudentDisplayServiceModel> GetStudentToDisplayAsync(string pin)
        {
            var student = await this.context.Students
                .Include(x => x.Class).ThenInclude(x => x.Subjects).ThenInclude(x => x.Subject)
                .Include(x => x.Class.ClassTeacher)
                .Include(x => x.Grades)
                .Include(x => x.Remarks)
                .Include(x => x.Absences)
                .FirstOrDefaultAsync(s => s.PIN == pin);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            var serviceModel = new StudentDisplayServiceModel { Student = student.To<StudentServiceModel>() };

            var subjects = await this.context.SubjectClasses.Where(x => x.ClassId == student.ClassId).To<ClassSubjectServiceModel>().ToListAsync();

            var gradesForSubjects = new List<GradesForSubjectServiceModel>();

            foreach (var subject in subjects)
            {
                var gradesForSubjectModel = new GradesForSubjectServiceModel { Subject = subject };

                var firstTermGrades = student.Grades.Where(x => x.Subject.Subject.Name == subject.Subject.Name && x.Term == Term.Първи && !x.IsTermGrade)
                    .Select(g => g.GradeValue).ToList();

                gradesForSubjectModel.FirstTermGrades = firstTermGrades;

                var firstTermGrade = student.Grades.SingleOrDefault(x => x.Subject.Subject.Name == subject.Subject.Name && x.Term == Term.Първи && x.IsTermGrade);

                if (firstTermGrade == null)
                {
                    gradesForSubjectModel.FirstTermGrade = "Няма";
                }
                else
                {
                    gradesForSubjectModel.FirstTermGrade = firstTermGrade.GradeValue;
                }

                var secondTermGrades = student.Grades.Where(x => x.Subject.Subject.Name == subject.Subject.Name && x.Term == Term.Втори && !x.IsTermGrade)
                    .Select(g => g.GradeValue).ToList();

                gradesForSubjectModel.SecondTermGrades = secondTermGrades;

                var secondTermGrade = student.Grades.SingleOrDefault(x => x.Subject.Subject.Name == subject.Subject.Name && x.Term == Term.Втори && x.IsTermGrade);

                if (secondTermGrade == null)
                {
                    gradesForSubjectModel.SecondTermGrade = "Няма";
                }
                else
                {
                    gradesForSubjectModel.SecondTermGrade = secondTermGrade.GradeValue;
                }

                gradesForSubjects.Add(gradesForSubjectModel);
            }

            serviceModel.GradesForSubject = gradesForSubjects;

            var absences = student.Absences.To<AbsenceServiceModel>().ToList();

            serviceModel.Absences = absences;

            var remarks = student.Remarks.To<RemarkServiceModel>().ToList();

            serviceModel.Remarks = remarks;

            return serviceModel;
        }

        public async Task<List<ClassSubjectServiceModel>> GetStudentSubjectsAsync(string id)
        {
            var student = await this.context.Students.Include(c => c.Class).ThenInclude(sc => sc.Subjects).ThenInclude(s => s.Subject)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            var classEntity = student.Class;

            return classEntity.Subjects
                .OrderBy(x => x.Subject.Name)
                .To<ClassSubjectServiceModel>().ToList();
        }

        public async Task<StudentServiceModel> GetStudentByIdAsync(string studentId)
        {
            var student = await this.context.Students.SingleOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            return student.To<StudentServiceModel>();
        }

        public async Task<List<StudentServiceModel>> GetAllStudentsInClassAsync(string classId)
        {
            var classFromDb = await this.context.Classes.SingleOrDefaultAsync(x => x.Id == classId);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            var students = await this.context.Students.Where(s => s.ClassId == classId)
                .OrderBy(x => x.FullName)
                .To<StudentServiceModel>().ToListAsync();

            return students;
        }

        public async Task<bool> CreateStudentAsync(StudentServiceModel model)
        {
            Student student = AutoMapper.Mapper.Map<Student>(model);

            Class classFromDb = await this.context.Classes.SingleOrDefaultAsync(c => c.Id == model.ClassId);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            classFromDb.Students.Add(student);

            this.context.Classes.Update(classFromDb);

            await this.context.Students.AddAsync(student);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditStudentAsync(StudentServiceModel model)
        {
            Student student = await this.context.Students.Include(x => x.Class)
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (student.FullName != model.FullName)
            {
                student.FullName = model.FullName;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.PIN != model.PIN)
            {
                student.PIN = model.PIN;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.Citizenship != model.Citizenship)
            {
                student.Citizenship = model.Citizenship;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.DateOfBirth != model.DateOfBirth)
            {
                student.DateOfBirth = model.DateOfBirth;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.PhoneNumber != model.PhoneNumber)
            {
                student.PhoneNumber = model.PhoneNumber;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.Town != model.Town)
            {
                student.Town = model.Town;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.Municipality != model.Municipality)
            {
                student.Municipality = model.Municipality;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.Region != model.Region)
            {
                student.Region = model.Region;

                student.ModifiedOn = DateTime.UtcNow;
            }

            if (student.Address != model.Address)
            {
                student.Address = model.Address;

                student.ModifiedOn = DateTime.UtcNow;
            }

            this.context.Students.Update(student);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            Student student = await this.context.Students
                .Include(x => x.Class)
                .Include(x => x.Grades)
                .Include(x => x.Absences)
                .Include(x => x.Remarks)
                .SingleOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            student.IsDeleted = true;
            student.DeletedOn = DateTime.UtcNow;

            var grades = student.Grades;
            var absences = student.Absences;
            var remarks = student.Remarks;

            foreach (var grade in grades)
            {
                grade.IsDeleted = true;
                grade.DeletedOn = DateTime.UtcNow;

                this.context.Grades.Update(grade);
            }

            foreach (var absence in absences)
            {
                absence.IsDeleted = true;
                absence.DeletedOn = DateTime.UtcNow;

                this.context.Absences.Update(absence);
            }

            foreach (var remark in remarks)
            {
                remark.IsDeleted = true;
                remark.DeletedOn = DateTime.UtcNow;

                this.context.Remarks.Update(remark);
            }

            this.context.Students.Update(student);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
