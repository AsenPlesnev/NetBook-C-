namespace NetBook.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<UserServiceModel> GetAllUsersWithRoles()
        {
            var users = this.context.Users
                .OrderBy(x => x.FullName)
                .To<UserServiceModel>();

            return users;
        }

        public async Task<UserServiceModel> GetUserByIdAsync(string id)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.To<UserServiceModel>();
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            this.context.Users.Update(user);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<SelectListItem>> GetTeacherNames()
        {
            return await this.context.Users.Where(x => !x.IsClassTeacher && x.IsTeacher).Select(teacher => new SelectListItem
            {
                Text = teacher.FullName,
                Value = teacher.Id,
            }).ToListAsync();
        }

        public async Task<bool> AddUserAsTeacherAsync(string id)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.IsTeacher = true;

            this.context.Users.Update(user);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
