namespace NetBook.Services.Data.Absence
{
    using System.Linq;
    using System.Threading.Tasks;

    using NetBook.Services.Models;

    public interface IAbsenceService
    {
        IQueryable<AbsenceServiceModel> GetAllAbsences();

        Task<bool> CreateAbsenceAsync(AbsenceServiceModel model);

        Task<bool> DeleteAbsenceAsync(string id);
    }
}
