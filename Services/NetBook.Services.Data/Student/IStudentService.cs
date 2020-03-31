namespace NetBook.Services.Data.Student
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using NetBook.Data.Models;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public interface IStudentService : IMapTo<Student>, IMapFrom<Student>
    {
        IQueryable<StudentServiceModel> GetAllStudents();

        Task<StudentDisplayServiceModel> GetStudentToDisplayAsync(string pin);

        Task<List<ClassSubjectServiceModel>> GetStudentSubjectsAsync(string id);

        Task<StudentServiceModel> GetStudentByIdAsync(string studentId);

        Task<List<StudentServiceModel>> GetAllStudentsInClassAsync(string classId);

        Task<bool> CreateStudentAsync(StudentServiceModel model);

        Task<bool> EditStudentAsync(StudentServiceModel model);

        Task<bool> DeleteStudentAsync(string studentId);
    }
}
