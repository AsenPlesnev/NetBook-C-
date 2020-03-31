namespace NetBook.Services.Data.Grade
{
    using System.Linq;
    using System.Threading.Tasks;

    using NetBook.Services.Models;

    public interface IGradeService
    {
        IQueryable<GradeServiceModel> GetAllGrades();

        IQueryable<GradeServiceModel> GetGradesForSubject(string studentId, string subjectId);

        Task<GradeServiceModel> GetGradeByIdAsync(string id);

        Task<bool> CreateGradeAsync(GradeServiceModel model);

        Task<bool> EditGradeAsync(GradeServiceModel model);

        Task<bool> DeleteGradeAsync(string id);
    }
}
