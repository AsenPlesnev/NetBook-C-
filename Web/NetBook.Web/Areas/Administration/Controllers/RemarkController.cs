namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Services.Data.Remark;
    using NetBook.Services.Mapping;
    using NetBook.Web.Paging;
    using NetBook.Web.ViewModels.Remark;

    [Authorize(Roles = "Administrator")]
    public class RemarkController : AdministrationController
    {
        private readonly IRemarkService remarkService;

        public RemarkController(IRemarkService remarkService)
        {
            this.remarkService = remarkService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var remarks = this.remarkService.GetAllRemarks().To<RemarkAllViewModel>();

            int pageSize = 8;

            return this.View(await PaginatedList<RemarkAllViewModel>.CreateAsync(remarks, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var result = await this.remarkService.DeleteRemarkAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
