namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Services.Data.Subject;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using NetBook.Web.InputModels.Subject;
    using NetBook.Web.ViewModels.Subject;

    [Authorize(Roles = "Administrator")]
    public class SubjectsController : AdministrationController
    {
        private readonly ISubjectService subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> All()
        {
            List<SubjectAllViewModel> subjects = await this.subjectService.GetAllSubjects().To<SubjectAllViewModel>().ToListAsync();

            return this.View(subjects);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubjectCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                SubjectServiceModel serviceModel = model.To<SubjectServiceModel>();

                bool result = await this.subjectService.CreateSubjectAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            return this.View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var subjectServiceModel = await this.subjectService.GetSubjectByIdAsync(id);

            var subject = subjectServiceModel.To<SubjectEditInputModel>();

            return this.View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubjectEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<SubjectServiceModel>();

                bool result = await this.subjectService.EditSubjectAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            bool result = await this.subjectService.DeleteSubjectAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
