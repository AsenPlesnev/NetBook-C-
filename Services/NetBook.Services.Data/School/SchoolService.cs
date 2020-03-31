namespace NetBook.Services.Data.School
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Data.Common.Repositories;
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext context;

        public SchoolService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<SchoolServiceModel> GetSchoolAsync()
        {
            return await this.context.School.To<SchoolServiceModel>().SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(SchoolServiceModel model)
        {
            var schoolToUpdate = AutoMapper.Mapper.Map<School>(model);

            var schoolToDelete = this.context.School.FirstOrDefault();

            this.context.School.Remove(schoolToDelete);

            await this.context.School.AddAsync(schoolToUpdate);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
