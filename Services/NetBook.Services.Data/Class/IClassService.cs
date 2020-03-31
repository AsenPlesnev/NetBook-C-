namespace NetBook.Services.Data.Class
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using NetBook.Services.Models;

    public interface IClassService
    {
        IQueryable<ClassServiceModel> GetAllClasses();

        ClassServiceModel GetClassById(string id);

        Task<string> GetClassNameByIdAsync(string classId);

        Task<List<SelectListItem>> GetStudentClassDropdownAsync(string id);

        Task<List<SelectListItem>> GetClassesDropdownAsync();

        List<SelectListItem> GetClassNumbersDropdown();

        Task<bool> CreateClassAsync(ClassServiceModel model);

        Task<bool> EditClassAsync(ClassServiceModel model);

        Task<bool> DeleteClassAsync(string id);

        IQueryable<ClassSubjectServiceModel> GetAllSubjectClasses(string id);

        Task<List<SelectListItem>> GetAllSubjectsInClassDropdownAsync(string id);

        Task<ClassSubjectServiceModel> GetSubjectById(string id);

        Task<bool> CreateSubjectAsync(ClassSubjectServiceModel model);

        Task<bool> EditSubjectAsync(ClassSubjectServiceModel model);

        Task<bool> DeleteSubjectAsync(string id);

        Task<List<SelectListItem>> GetAllStudentsInClassDropdownAsync(string id);

        IQueryable<AbsenceServiceModel> GetAllAbsences(string id);

        IQueryable<RemarkServiceModel> GetAllRemarks(string id);
    }
}
