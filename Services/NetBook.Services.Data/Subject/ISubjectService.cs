namespace NetBook.Services.Data.Subject
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using NetBook.Services.Models;

    public interface ISubjectService
    {
        IQueryable<SubjectServiceModel> GetAllSubjects();

        Task<SubjectServiceModel> GetSubjectByIdAsync(string id);

        Task<List<SelectListItem>> GetSubjectNamesAsync(string classId);

        Task<List<SelectListItem>> GetSubjectsDropdownAsync();

        Task<bool> CreateSubjectAsync(SubjectServiceModel model);

        Task<bool> EditSubjectAsync(SubjectServiceModel model);

        Task<bool> DeleteSubjectAsync(string id);
    }
}
