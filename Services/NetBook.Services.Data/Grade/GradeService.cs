namespace NetBook.Services.Data.Grade
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext context;

        public GradeService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<GradeServiceModel> GetAllGrades()
        {
            return this.context.Grades
                .OrderBy(x => x.Term)
                .ThenBy(x => x.IsTermGrade)
                .ThenBy(x => x.GradeValue)
                .To<GradeServiceModel>();
        }

        public IQueryable<GradeServiceModel> GetGradesForSubject(string studentId, string subjectId)
        {
            var grades = this.context.Grades.Where(x => x.SubjectId == subjectId && x.StudentId == studentId)
                .OrderBy(x => x.Term)
                .ThenBy(x => x.IsTermGrade)
                .ThenBy(x => x.GradeValue)
                .To<GradeServiceModel>();

            var student = this.context.Students.Find(studentId);
            var subject = this.context.SubjectClasses.Find(subjectId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return grades;
        }

        public async Task<GradeServiceModel> GetGradeByIdAsync(string id)
        {
            var grade = await this.context.Grades
                .Include(x => x.Student)
                .Include(x => x.Subject).ThenInclude(x => x.Subject)
                .SingleOrDefaultAsync(g => g.Id == id);

            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return grade.To<GradeServiceModel>();
        }

        public async Task<bool> CreateGradeAsync(GradeServiceModel model)
        {
            var grade = Mapper.Map<Grade>(model);

            var student = await this.context.Students.Where(x => !x.IsDeleted).SingleOrDefaultAsync(s => s.Id == model.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (grade.IsTermGrade)
            {
                var termGradeFromContext = await this.context.Grades
                    .Where(x => x.Term == grade.Term)
                    .Where(x => x.SubjectId == grade.SubjectId)
                    .Where(x => x.StudentId == grade.StudentId)
                    .SingleOrDefaultAsync(g => g.IsTermGrade);

                if (termGradeFromContext != null)
                {
                    return false;
                }
            }

            student.Grades.Add(grade);

            this.context.Students.Update(student);

            await this.context.Grades.AddAsync(grade);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditGradeAsync(GradeServiceModel model)
        {
            var grade = await this.context.Grades.Where(x => !x.IsDeleted).SingleOrDefaultAsync(g => g.Id == model.Id);

            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            if (grade.GradeValue != model.GradeValue)
            {
                grade.GradeValue = model.GradeValue;

                grade.ModifiedOn = DateTime.UtcNow;
            }

            if (grade.Term != model.Term)
            {
                grade.Term = model.Term;

                grade.ModifiedOn = DateTime.UtcNow;
            }

            if (grade.IsTermGrade != model.IsTermGrade)
            {
                if (model.IsTermGrade)
                {
                    var termGradeFromContext = await this.context.Grades
                        .Where(x => x.Term == grade.Term)
                        .Where(x => x.SubjectId == grade.SubjectId)
                        .Where(x => x.StudentId == grade.StudentId)
                        .SingleOrDefaultAsync(g => g.IsTermGrade);

                    if (termGradeFromContext != null)
                    {
                        return false;
                    }
                }

                grade.IsTermGrade = model.IsTermGrade;

                grade.ModifiedOn = DateTime.UtcNow;
            }

            this.context.Grades.Update(grade);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteGradeAsync(string id)
        {
            var grade = await this.context.Grades.Where(x => !x.IsDeleted).SingleOrDefaultAsync(g => g.Id == id);

            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            grade.IsDeleted = true;
            grade.DeletedOn = DateTime.UtcNow;

            this.context.Grades.Update(grade);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
