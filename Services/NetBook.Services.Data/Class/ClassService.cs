namespace NetBook.Services.Data.Class
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext context;

        public ClassService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ClassServiceModel> GetAllClasses()
        {
            return this.context.Classes.Where(c => !c.IsDeleted)
                .OrderBy(x => x.ClassNumber)
                .ThenBy(x => x.ClassLetter)
                .To<ClassServiceModel>();
        }

        public ClassServiceModel GetClassById(string id)
        {
            var classEntity = this.GetAllClasses().FirstOrDefault(x => x.Id == id);

            if (classEntity == null)
            {
                throw new ArgumentNullException(nameof(classEntity));
            }

            return classEntity;
        }

        public async Task<string> GetClassNameByIdAsync(string classId)
        {
            var classFromDb = await this.context.Classes.Where(x => !x.IsDeleted).SingleOrDefaultAsync(c => c.Id == classId);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            return $"{classFromDb.ClassNumber} {classFromDb.ClassLetter}";
        }

        public async Task<List<SelectListItem>> GetStudentClassDropdownAsync(string id)
        {
            var student = await this.context.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            var classId = student.ClassId;

            return await this.context.Classes.Where(c => c.Id == classId).Select(classEntity => new SelectListItem
            {
                Text = $"{classEntity.ClassNumber} {classEntity.ClassLetter}",
                Value = classEntity.Id,
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetClassesDropdownAsync()
        {
            return await this.context.Classes.Select(classEntity => new SelectListItem
            {
                Text = $"{classEntity.ClassNumber} {classEntity.ClassLetter}",
                Value = classEntity.Id,
            }).ToListAsync();
        }

        public List<SelectListItem> GetClassNumbersDropdown()
        {
            return new List<SelectListItem>
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
                },new SelectListItem
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
        }

        public async Task<bool> CreateClassAsync(ClassServiceModel model)
        {
            var classToAdd = AutoMapper.Mapper.Map<Class>(model);

            var teacher = await this.context.Users.SingleOrDefaultAsync(x => x.Id == model.ClassTeacherId);

            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            teacher.IsClassTeacher = true;
            teacher.ClassId = classToAdd.Id;

            this.context.Users.Update(teacher);

            await this.context.Classes.AddAsync(classToAdd);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditClassAsync(ClassServiceModel model)
        {
            var classFromDb = await this.context.Classes.Where(x => !x.IsDeleted).SingleOrDefaultAsync(c => c.Id == model.Id);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            if (classFromDb.ClassNumber != model.ClassNumber)
            {
                classFromDb.ClassNumber = model.ClassNumber;

                classFromDb.ModifiedOn = DateTime.UtcNow;
            }

            if (classFromDb.ClassLetter != model.ClassLetter)
            {
                classFromDb.ClassLetter = model.ClassLetter;

                classFromDb.ModifiedOn = DateTime.UtcNow;
            }

            if (classFromDb.SchoolYearStart != model.SchoolYearStart)
            {
                classFromDb.SchoolYearStart = model.SchoolYearStart;

                classFromDb.ModifiedOn = DateTime.UtcNow;
            }

            if (classFromDb.SchoolYearEnd != model.SchoolYearEnd)
            {
                classFromDb.SchoolYearEnd = model.SchoolYearEnd;

                classFromDb.ModifiedOn = DateTime.UtcNow;
            }

            if ((classFromDb.SchoolYearEnd - classFromDb.SchoolYearStart) != 1)
            {
                return false;
            }

            this.context.Classes.Update(classFromDb);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteClassAsync(string id)
        {
            var classFromDb = await this.context.Classes.Include(t => t.ClassTeacher)
                .Include(sl => sl.Students)
                .Include(sub => sub.Subjects)
                .Where(x => !x.IsDeleted)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            classFromDb.IsDeleted = true;
            classFromDb.DeletedOn = DateTime.UtcNow;

            var students = classFromDb.Students;

            foreach (var student in students)
            {
                student.IsDeleted = true;
                student.DeletedOn = DateTime.UtcNow;

                this.context.Students.Update(student);
            }

            var subjects = classFromDb.Subjects;

            foreach (var subject in subjects)
            {
                subject.IsDeleted = true;
                subject.DeletedOn = DateTime.UtcNow;

                this.context.SubjectClasses.Update(subject);
            }

            var teacher = classFromDb.ClassTeacher;

            teacher.IsDeleted = true;
            teacher.DeletedOn = DateTime.UtcNow;

            this.context.Users.Update(teacher);

            this.context.Classes.Update(classFromDb);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ClassSubjectServiceModel> GetAllSubjectClasses(string id)
        {
            return this.context.SubjectClasses.Include(x => x.Subject)
                .OrderBy(x => x.Subject.Name)
                .To<ClassSubjectServiceModel>().Where(x => x.ClassId == id);
        }

        public async Task<List<SelectListItem>> GetAllSubjectsInClassDropdownAsync(string id)
        {
            var classFromDb = await this.context.Classes.Include(s => s.Subjects).ThenInclude(sc => sc.Subject)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            var subjects = classFromDb.Subjects;

            return subjects.Select(subject => new SelectListItem
            {
                Text = subject.Subject.Name,
                Value = subject.Id,
            }).ToList();
        }

        public async Task<ClassSubjectServiceModel> GetSubjectById(string id)
        {
            var subject = await this.context.SubjectClasses.Include(x => x.Subject).To<ClassSubjectServiceModel>().SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return subject;
        }

        public async Task<bool> CreateSubjectAsync(ClassSubjectServiceModel model)
        {
            var classEntity = await this.context.Classes.Include(c => c.Subjects).SingleOrDefaultAsync(x => x.Id == model.ClassId);

            if (classEntity == null)
            {
                throw new ArgumentNullException(nameof(classEntity));
            }

            var subject = AutoMapper.Mapper.Map<SubjectClass>(model);

            classEntity.Subjects.Add(subject);

            this.context.Classes.Update(classEntity);

            await this.context.SubjectClasses.AddAsync(subject);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditSubjectAsync(ClassSubjectServiceModel model)
        {
            var subject = await this.context.SubjectClasses.Where(s => !s.IsDeleted)
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (subject.Workload != model.Workload)
            {
                subject.Workload = model.Workload;

                subject.ModifiedOn = DateTime.UtcNow;
            }

            this.context.SubjectClasses.Update(subject);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteSubjectAsync(string id)
        {
            var subject = await this.context.SubjectClasses.Include(x => x.Class).Where(s => !s.IsDeleted)
                .SingleOrDefaultAsync(sub => sub.Id == id);

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            subject.IsDeleted = true;
            subject.DeletedOn = DateTime.UtcNow;

            this.context.SubjectClasses.Update(subject);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<SelectListItem>> GetAllStudentsInClassDropdownAsync(string id)
        {
            var classFromDb = await this.context.Classes.SingleOrDefaultAsync(x => x.Id == id);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            var students = await this.context.Students.Where(x => x.ClassId == id).ToListAsync();

            return students.Select(student => new SelectListItem
            {
                Text = student.FullName,
                Value = student.Id,
            }).ToList();
        }

        public IQueryable<AbsenceServiceModel> GetAllAbsences(string id)
        {
            var student = this.context.Students.Find(id);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            var absences = this.context.Absences.Include(x => x.Subject)
                .Where(x => !x.IsDeleted && x.StudentId == id)
                .OrderBy(x => x.Subject.Subject.Name)
                .To<AbsenceServiceModel>();

            return absences;
        }

        public IQueryable<RemarkServiceModel> GetAllRemarks(string id)
        {
            var student = this.context.Students.Find(id);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            var remarks = this.context.Remarks.Include(s => s.Subject)
                .Where(x => !x.IsDeleted && x.StudentId == id)
                .OrderBy(x => x.Subject.Subject.Name)
                .ThenBy(x => x.Text)
                .To<RemarkServiceModel>();

            return remarks;
        }
    }
}
