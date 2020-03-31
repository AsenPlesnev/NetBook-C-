namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using NetBook.Services.Data.User;
    using NetBook.Services.Mapping;
    using NetBook.Web.ViewModels.User;

    [Authorize(Roles = "Administrator")]
    public class UserController : AdministrationController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult All()
        {
            List<UserAllViewModel> users = this.userService.GetAllUsersWithRoles().To<UserAllViewModel>().ToList();

            return this.View(users);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            bool result = await this.userService.DeleteUserAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
