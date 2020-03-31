namespace NetBook.Services.Data.Subject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using NetBook.Web.ViewModels;

    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext context;

        public SubjectService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<SubjectServiceModel> GetAllSubjects()
        {
            return this.context.Subjects.Where(s => !s.IsDeleted)
                .OrderBy(x => x.Name)
                .To<SubjectServiceModel>();
        }

        public async Task<SubjectServiceModel> GetSubjectByIdAsync(string id)
        {
            var subjectFromDb = await this.context.Subjects.To<SubjectServiceModel>().SingleOrDefaultAsync(x => x.Id == id);

            if (subjectFromDb == null)
            {
                throw new ArgumentNullException(nameof(subjectFromDb));
            }

            return subjectFromDb;
        }

        public async Task<List<SelectListItem>> GetSubjectNamesAsync(string classId)
        {
            Class classFromDb = await this.context.Classes.Include(x => x.Subjects).ThenInclude(s => s.Subject)
                .SingleOrDefaultAsync(c => c.Id == classId);

            if (classFromDb == null)
            {
                throw new ArgumentNullException(nameof(classFromDb));
            }

            List<string> subjectNames = await this.context.Subjects.Select(x => x.Name).ToListAsync();

            List<string> classSubjectNames = classFromDb.Subjects.Select(x => x.Subject.Name).ToList();

            List<string> validSubjectNames = subjectNames.Except(classSubjectNames).ToList();

            List<Subject> validSubjects = new List<Subject>();

            foreach (var validSubjectName in validSubjectNames)
            {
                Subject subject = await this.context.Subjects.SingleOrDefaultAsync(x => x.Name == validSubjectName);

                validSubjects.Add(subject);
            }

            return validSubjects.Select(subject => new SelectListItem
                {
                    Text = subject.Name,
                    Value = subject.Id,
                })
                .ToList();
        }

        public async Task<List<SelectListItem>> GetSubjectsDropdownAsync()
        {
            var subjects = this.context.Subjects;

            return await subjects.Select(subject => new SelectListItem
            {
                Text = subject.Name,
                Value = subject.Id,
            }).ToListAsync();
        }

        public async Task<bool> CreateSubjectAsync(SubjectServiceModel model)
        {
            Subject subjectToAdd = AutoMapper.Mapper.Map<Subject>(model);

            var subjectWithSameName = await this.context.Subjects.SingleOrDefaultAsync(s => s.Name == subjectToAdd.Name);

            if (subjectWithSameName != null)
            {
                return false;
            }

            await this.context.Subjects.AddAsync(subjectToAdd);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditSubjectAsync(SubjectServiceModel model)
        {
            Subject subjectFromDb = await this.context.Subjects.SingleOrDefaultAsync(s => s.Id == model.Id);

            if (subjectFromDb == null)
            {
                throw new ArgumentNullException(nameof(subjectFromDb));
            }

            if (model.Name != subjectFromDb.Name)
            {
                var sameSubjectName = await this.context.Subjects.SingleOrDefaultAsync(x => x.Name == model.Name);

                if (sameSubjectName != null)
                {
                    return false;
                }

                subjectFromDb.Name = model.Name;

                subjectFromDb.ModifiedOn = DateTime.UtcNow;
            }

            this.context.Subjects.Update(subjectFromDb);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteSubjectAsync(string id)
        {
            Subject subjectFromDb = await this.context.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subjectFromDb == null)
            {
                throw new ArgumentNullException(nameof(subjectFromDb));
            }

            subjectFromDb.IsDeleted = true;
            subjectFromDb.DeletedOn = DateTime.UtcNow;

            this.context.Subjects.Update(subjectFromDb);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
