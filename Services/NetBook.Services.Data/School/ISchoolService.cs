namespace NetBook.Services.Data.School
{
    using System.Threading.Tasks;

    using NetBook.Data.Models;
    using NetBook.Services.Models;

    public interface ISchoolService
    {
        Task<SchoolServiceModel> GetSchoolAsync();

        Task<bool> UpdateAsync(SchoolServiceModel model);
    }
}
