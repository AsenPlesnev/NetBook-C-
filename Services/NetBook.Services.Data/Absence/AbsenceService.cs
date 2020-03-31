namespace NetBook.Services.Data.Absence
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

    public class AbsenceService : IAbsenceService
    {
        private readonly ApplicationDbContext context;

        public AbsenceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<AbsenceServiceModel> GetAllAbsences()
        {
            var absences = this.context.Absences
                .Include(x => x.Student).ThenInclude(x => x.Class)
                .Include(x => x.Subject).ThenInclude(x => x.Subject)
                .OrderBy(x => x.Student.Class.ClassNumber)
                .ThenBy(x => x.Student.Class.ClassLetter)
                .ThenBy(x => x.Student.FullName)
                .ThenBy(x => x.Subject.Subject.Name)
                .To<AbsenceServiceModel>();

            return absences;
        }

        public async Task<bool> CreateAbsenceAsync(AbsenceServiceModel model)
        {
            var absence = Mapper.Map<Absence>(model);

            var student = await this.context.Students.Where(x => !x.IsDeleted).FirstOrDefaultAsync(s => s.Id == absence.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            student.Absences.Add(absence);

            this.context.Students.Update(student);

            await this.context.Absences.AddAsync(absence);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAbsenceAsync(string id)
        {
            var absence = await this.context.Absences.Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.Id == id);

            if (absence == null)
            {
                throw new ArgumentNullException(nameof(absence));
            }

            absence.IsDeleted = true;
            absence.DeletedOn = DateTime.UtcNow;

            this.context.Absences.Update(absence);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
