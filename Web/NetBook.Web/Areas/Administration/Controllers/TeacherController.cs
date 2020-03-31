using Microsoft.AspNetCore.Authorization;
using NetBook.Common;

namespace NetBook.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using NetBook.Common;
    using NetBook.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class TeacherController : BaseController
    {
        public IActionResult All()
        {
            return View();
        }
    }
}