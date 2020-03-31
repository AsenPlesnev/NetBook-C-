namespace NetBook.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Data.Subject;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using NetBook.Web.InputModels.Student;
    using NetBook.Web.Paging;
    using NetBook.Web.ViewModels.Student;

    [Authorize(Roles = "Administrator")]
    public class StudentsController : AdministrationController
    {
        private readonly IStudentService studentService;
        private readonly IClassService classService;
        private readonly ISubjectService subjectService;

        public StudentsController(IStudentService studentService, IClassService classService, ISubjectService subjectService)
        {
            this.studentService = studentService;
            this.classService = classService;
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var students = this.studentService.GetAllStudents().To<StudentAllViewModel>();

            int pageSize = 8;

            return this.View(await PaginatedList<StudentAllViewModel>.CreateAsync(students, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Create()
        {
            var classes = await this.classService.GetClassesDropdownAsync();

            this.ViewBag.ClassesNames = classes;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                StudentServiceModel serviceModel = model.To<StudentServiceModel>();

                bool result = await this.studentService.CreateStudentAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            var classes = await this.classService.GetClassesDropdownAsync();

            this.ViewBag.ClassesNames = classes;

            return this.View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var studentClass = await this.classService.GetStudentClassDropdownAsync(id);

            this.ViewBag.StudentClass = studentClass;

            var student = await this.studentService.GetStudentByIdAsync(id);

            var model = student.To<StudentEditInputModel>();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                StudentServiceModel serviceModel = model.To<StudentServiceModel>();

                bool result = await this.studentService.EditStudentAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            var studentClass = await this.classService.GetStudentClassDropdownAsync(model.Id);

            this.ViewBag.StudentClass = studentClass;

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            bool result = await this.studentService.DeleteStudentAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }
    }
}
