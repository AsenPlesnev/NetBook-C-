using System.Linq;

namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using NetBook.Services.Data.Absence;
    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Subject;
    using NetBook.Services.Mapping;
    using NetBook.Web.Paging;
    using NetBook.Web.ViewModels.Absence;

    [Authorize(Roles = "Administrator")]
    public class AbsenceController : AdministrationController
    {
        private readonly IAbsenceService absenceService;
        private readonly IClassService classService;
        private readonly ISubjectService subjectService;

        public AbsenceController(IAbsenceService absenceService, IClassService classService, ISubjectService subjectService)
        {
            this.absenceService = absenceService;
            this.classService = classService;
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var absences = this.absenceService.GetAllAbsences();

            // Changed it from absences.To<AbsenceAllViewModel>(); because it caused a strange error
            var model = absences.Select(x => x.To<AbsenceAllViewModel>());

            int pageSize = 8;

            return this.View(await PaginatedList<AbsenceAllViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var result = await this.absenceService.DeleteAbsenceAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
