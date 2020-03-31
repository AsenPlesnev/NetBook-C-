using System.Linq;

namespace NetBook.Services.Data.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using NetBook.Services.Models;

    public interface IUserService
    {
        IQueryable<UserServiceModel> GetAllUsersWithRoles();

        Task<UserServiceModel> GetUserByIdAsync(string id);

        Task<List<SelectListItem>> GetTeacherNames();

        Task<bool> AddUserAsTeacherAsync(string id);

        Task<bool> DeleteUserAsync(string id);
    }
}
