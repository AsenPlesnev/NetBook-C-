namespace NetBook.Services.Data.Remark
{
    using System.Linq;
    using System.Threading.Tasks;

    using NetBook.Services.Models;

    public interface IRemarkService
    {
        IQueryable<RemarkServiceModel> GetAllRemarks();

        Task<RemarkServiceModel> GetRemarkByIdAsync(string id);

        Task<bool> CreateRemarkAsync(RemarkServiceModel model);

        Task<bool> DeleteRemarkAsync(string id);
    }
}
