namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Grade;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using NetBook.Web.InputModels.Grade;
    using NetBook.Web.Paging;
    using NetBook.Web.ViewModels.Grade;

    [Authorize(Roles = "Administrator")]
    public class GradeController : AdministrationController
    {
        private readonly IGradeService gradeService;
        private readonly IClassService classService;
        private readonly IStudentService studentService;

        public GradeController(IGradeService gradeService, IClassService classService, IStudentService studentService)
        {
            this.gradeService = gradeService;
            this.classService = classService;
            this.studentService = studentService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var grades = this.gradeService.GetAllGrades().To<GradeAllViewModel>();

            int pageSize = 8;

            return this.View(await PaginatedList<GradeAllViewModel>.CreateAsync(grades, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var serviceModel = await this.gradeService.GetGradeByIdAsync(id);

            var model = serviceModel.To<GradeEditInputModel>();

            var student = serviceModel.Student;
            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = model.StudentId;

            var subject = serviceModel.Subject;
            this.ViewBag.SubjectName = subject.Subject.Name;
            this.ViewBag.SubjectId = model.SubjectId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GradeEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<GradeServiceModel>();

                var studentId = model.StudentId;
                var subjectId = model.SubjectId;

                var subject = await this.classService.GetSubjectById(subjectId);
                var student = await this.studentService.GetStudentByIdAsync(studentId);

                var result = await this.gradeService.EditGradeAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }

                this.ViewBag.StudentId = studentId;
                this.ViewBag.SubjectId = subjectId;

                this.ViewBag.SubjectName = subject.Subject.Name;
                this.ViewBag.StudentName = student.FullName;

                return this.View(model);
            }

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var result = await this.gradeService.DeleteGradeAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
