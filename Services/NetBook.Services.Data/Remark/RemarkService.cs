namespace NetBook.Services.Data.Remark
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

    public class RemarkService : IRemarkService
    {
        private readonly ApplicationDbContext context;

        public RemarkService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<RemarkServiceModel> GetAllRemarks()
        {
            return this.context.Remarks
                .Include(x => x.Student)
                .Include(x => x.Subject).ThenInclude(x => x.Subject)
                .OrderBy(x => x.Student.FullName)
                .ThenBy(x => x.Subject.Subject.Name)
                .To<RemarkServiceModel>();
        }

        public async Task<RemarkServiceModel> GetRemarkByIdAsync(string id)
        {
            var remark = await this.context.Remarks.Include(x => x.Student).Include(x => x.Subject)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (remark == null)
            {
                throw new ArgumentNullException(nameof(remark));
            }

            return remark.To<RemarkServiceModel>();
        }

        public async Task<bool> CreateRemarkAsync(RemarkServiceModel model)
        {
            var remark = Mapper.Map<Remark>(model);

            var student = await this.context.Students.Include(c => c.Class).SingleOrDefaultAsync(s => s.Id == model.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            student.Remarks.Add(remark);

            this.context.Remarks.Update(remark);

            await this.context.Remarks.AddAsync(remark);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteRemarkAsync(string id)
        {
            var remark = await this.context.Remarks.Where(x => !x.IsDeleted).SingleOrDefaultAsync(r => r.Id == id);

            if (remark == null)
            {
                throw new ArgumentNullException(nameof(remark));
            }

            remark.IsDeleted = true;
            remark.DeletedOn = DateTime.UtcNow;

            this.context.Remarks.Update(remark);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
