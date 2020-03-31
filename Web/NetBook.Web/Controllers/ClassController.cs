namespace NetBook.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NetBook.Services.Data.Absence;
    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Grade;
    using NetBook.Services.Data.Remark;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Data.Subject;
    using NetBook.Services.Data.User;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using NetBook.Web.InputModels.Absence;
    using NetBook.Web.InputModels.Class;
    using NetBook.Web.InputModels.Grade;
    using NetBook.Web.InputModels.Remark;
    using NetBook.Web.InputModels.Student;
    using NetBook.Web.Paging;
    using NetBook.Web.ViewModels.Absence;
    using NetBook.Web.ViewModels.Class;
    using NetBook.Web.ViewModels.Grade;
    using NetBook.Web.ViewModels.Remark;

    [Authorize]
    public class ClassController : BaseController
    {
        private readonly IUserService userService;
        private readonly IClassService classService;
        private readonly IStudentService studentService;
        private readonly ISubjectService subjectService;
        private readonly IAbsenceService absenceService;
        private readonly IRemarkService remarkService;
        private readonly IGradeService gradeService;

        public ClassController(IUserService userService, IClassService classService, IStudentService studentService, ISubjectService subjectService, IAbsenceService absenceService, IRemarkService remarkService, IGradeService gradeService)
        {
            this.userService = userService;
            this.classService = classService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.absenceService = absenceService;
            this.remarkService = remarkService;
            this.gradeService = gradeService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var classes = this.classService.GetAllClasses().To<ClassAllViewModel>();

            int pageSize = 2;

            return this.View(await PaginatedList<ClassAllViewModel>.CreateAsync(classes, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Create()
        {
            var classNumbers = this.classService.GetClassNumbersDropdown();
            var teacherNames = await this.userService.GetTeacherNames();

            this.ViewBag.ClassNumbers = classNumbers;
            this.ViewBag.TeacherNames = teacherNames;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClassCreateInputModel model)
        {
            if (this.ModelState.IsValid && (model.SchoolYearEnd - model.SchoolYearStart == 1))
            {
                var serviceModel = model.To<ClassServiceModel>();

                var result = await this.classService.CreateClassAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            var classNumbers = this.classService.GetClassNumbersDropdown();
            var teacherNames = await this.userService.GetTeacherNames();

            this.ViewBag.ClassNumbers = classNumbers;
            this.ViewBag.TeacherNames = teacherNames;

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var classNumbers = this.classService.GetClassNumbersDropdown();

            this.ViewBag.ClassNumbers = classNumbers;

            var classModel = this.classService.GetClassById(id).To<ClassEditInputModel>();

            this.ViewBag.ClassNumber = classModel.ClassNumber;
            this.ViewBag.ClassLetter = classModel.ClassLetter;

            var teacher = await this.userService.GetUserByIdAsync(classModel.ClassTeacherId);

            this.ViewBag.ClassTeacherName = teacher.FullName;
            this.ViewBag.ClassTeacherId = classModel.ClassTeacherId;

            return this.View(classModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(ClassEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<ClassServiceModel>();

                var result = await this.classService.EditClassAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("All");
                }
            }

            var classNumbers = this.classService.GetClassNumbersDropdown();

            this.ViewBag.ClassNumbers = classNumbers;

            var classModel = this.classService.GetClassById(model.Id);

            this.ViewBag.ClassNumber = classModel.ClassNumber;
            this.ViewBag.ClassLetter = classModel.ClassLetter;

            this.ViewBag.ClassTeacherName = classModel.ClassTeacher.FullName;
            this.ViewBag.ClassTeacherId = classModel.ClassTeacherId;

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteClass(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var result = await this.classService.DeleteClassAsync(id);

            if (result)
            {
                return this.RedirectToAction("All");
            }

            return this.BadRequest();
        }

        public async Task<IActionResult> StudentsAll(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var className = await this.classService.GetClassNameByIdAsync(id);

            this.ViewBag.ClassName = className;
            this.ViewBag.ClassId = id;

            var serviceModels = await this.studentService.GetAllStudentsInClassAsync(id);

            var students = serviceModels.To<AllStudentsInClassViewModel>().ToList();

            return this.View(students);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditStudent(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var student = await this.studentService.GetStudentByIdAsync(id);

            var model = student.To<StudentEditInputModel>();

            var studentClass = await this.classService.GetStudentClassDropdownAsync(id);

            this.ViewBag.StudentClass = studentClass;

            string className = studentClass.First().Text;
            string classId = studentClass.First().Value;

            this.ViewBag.ClassName = className;
            this.ViewBag.ClassId = classId;

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditStudent(StudentEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<StudentServiceModel>();

                var result = await this.studentService.EditStudentAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("StudentsAll", new { id = model.ClassId });
                }
            }

            string classFullName = await this.classService.GetClassNameByIdAsync(model.ClassId);
            string classId = model.ClassId;

            this.ViewBag.ClassName = classFullName;
            this.ViewBag.ClassId = classId;

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var student = await this.studentService.GetStudentByIdAsync(id);

            var classId = student.ClassId;

            var result = await this.studentService.DeleteStudentAsync(id);

            if (result)
            {
                return this.RedirectToAction("StudentsAll", new { id = classId });
            }

            return this.BadRequest();
        }

        public async Task<IActionResult> SubjectsAll(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var className = await this.classService.GetClassNameByIdAsync(id);
            this.ViewBag.ClassId = id;
            this.ViewBag.ClassName = className;

            var subjects = await this.classService.GetAllSubjectClasses(id).To<SubjectInClassAllViewModel>()
                .ToListAsync();

            return this.View(subjects);
        }

        public async Task<IActionResult> CreateSubject(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            this.ViewBag.ClassId = id;

            var className = await this.classService.GetClassNameByIdAsync(id);
            this.ViewBag.ClassName = className;

            var subjects = await this.subjectService.GetSubjectNamesAsync(id);
            this.ViewBag.SubjectNames = subjects;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectInClassCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<ClassSubjectServiceModel>();

                var result = await this.classService.CreateSubjectAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("SubjectsAll", new { id = model.ClassId });
                }
            }

            this.ViewBag.ClassId = model.ClassId;

            var className = await this.classService.GetClassNameByIdAsync(model.ClassId);
            this.ViewBag.ClassName = className;

            var subjects = await this.subjectService.GetSubjectNamesAsync(model.ClassId);
            this.ViewBag.SubjectNames = subjects;

            return this.View(model);
        }

        public async Task<IActionResult> EditSubject(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var serviceModel = await this.classService.GetSubjectById(id);

            var model = serviceModel.To<SubjectInClassEditInputModel>();

            this.ViewBag.SubjectId = model.SubjectId;
            this.ViewBag.SubjectName = serviceModel.Subject.Name;
            this.ViewBag.ClassId = model.ClassId;

            var className = await this.classService.GetClassNameByIdAsync(model.ClassId);
            this.ViewBag.ClassName = className;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubject(SubjectInClassEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<ClassSubjectServiceModel>();

                var result = await this.classService.EditSubjectAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("SubjectsAll", new { id = model.ClassId });
                }
            }

            var subject = await this.classService.GetSubjectById(model.Id);

            this.ViewBag.SubjectId = model.SubjectId;
            this.ViewBag.SubjectName = subject.Subject.Name;
            this.ViewBag.ClassId = model.ClassId;

            var className = await this.classService.GetClassNameByIdAsync(model.ClassId);
            this.ViewBag.ClassName = className;

            return this.View(model);
        }

        public async Task<IActionResult> DeleteSubject(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var subject = await this.classService.GetSubjectById(id);

            var classId = subject.ClassId;

            var result = await this.classService.DeleteSubjectAsync(id);

            if (result)
            {
                return this.RedirectToAction("SubjectsAll", new { id = classId });
            }

            return this.BadRequest();
        }

        [Route("/Class/Students/AbsencesAll/{id}")]
        public async Task<IActionResult> AbsencesAll(string id, int? pageNumber)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var absences = this.classService.GetAllAbsences(id);

            var model = absences.Select(x => x.To<AbsenceAllViewModel>());

            var student = await this.studentService.GetStudentByIdAsync(id);

            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = id;
            this.ViewBag.ClassId = student.ClassId;

            int pageSize = 8;

            return this.View(await PaginatedList<AbsenceAllViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
        }

        [Route("/Class/Students/AbsenceCreate/{id}")]
        public async Task<IActionResult> AbsenceCreate(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var student = await this.studentService.GetStudentByIdAsync(id);
            var classId = student.ClassId;

            this.ViewBag.ClassId = classId;

            var subjects = await this.classService.GetAllSubjectsInClassDropdownAsync(classId);
            this.ViewBag.SubjectNames = subjects;

            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = id;

            this.ViewBag.ClassName = await this.classService.GetClassNameByIdAsync(classId);

            return this.View();
        }

        [HttpPost]
        [Route("/Class/Students/AbsenceCreate/{id}")]
        public async Task<IActionResult> AbsenceCreate(AbsenceCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<AbsenceServiceModel>();

                var result = await this.absenceService.CreateAbsenceAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("AbsencesAll", new { id = model.StudentId });
                }
            }

            this.ViewBag.ClassId = model.ClassId;

            var subjects = await this.classService.GetAllSubjectsInClassDropdownAsync(model.ClassId);
            this.ViewBag.SubjectNames = subjects;

            this.ViewBag.StudentId = model.StudentId;

            var student = await this.studentService.GetStudentByIdAsync(model.StudentId);
            this.ViewBag.StudentName = student.FullName;

            this.ViewBag.ClassName = await this.classService.GetClassNameByIdAsync(model.ClassId);

            return this.View(model);
        }

        public async Task<IActionResult> AbsenceDelete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var absence = this.absenceService.GetAllAbsences().FirstOrDefault(a => a.Id == id);

            if (absence == null)
            {
                throw new ArgumentNullException(nameof(absence));
            }

            var result = await this.absenceService.DeleteAbsenceAsync(id);

            if (result)
            {
                return this.RedirectToAction("AbsencesAll", new { id = absence.Student.Id });
            }

            return this.BadRequest();
        }

        [Route("/Class/Students/RemarksAll/{id}")]
        public async Task<IActionResult> RemarksAll(string id, int? pageNumber)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var remarks = this.classService.GetAllRemarks(id).To<RemarkAllViewModel>();

            var student = await this.studentService.GetStudentByIdAsync(id);

            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = id;
            this.ViewBag.ClassId = student.ClassId;

            int pageSize = 8;

            return this.View(await PaginatedList<RemarkAllViewModel>.CreateAsync(remarks, pageNumber ?? 1, pageSize));
        }

        [Route("/Class/Students/RemarkCreate/{id}")]
        public async Task<IActionResult> RemarkCreate(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var student = await this.studentService.GetStudentByIdAsync(id);
            var classId = student.ClassId;

            this.ViewBag.ClassId = classId;

            var subjects = await this.classService.GetAllSubjectsInClassDropdownAsync(classId);
            this.ViewBag.SubjectNames = subjects;

            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = id;

            this.ViewBag.ClassName = await this.classService.GetClassNameByIdAsync(classId);

            return this.View();
        }

        [HttpPost]
        [Route("/Class/Students/RemarkCreate/{id}")]
        public async Task<IActionResult> RemarkCreate(RemarkCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<RemarkServiceModel>();

                var result = await this.remarkService.CreateRemarkAsync(serviceModel);

                if (result)
                {
                    return this.RedirectToAction("RemarksAll", new { id = model.StudentId });
                }
            }

            var student = await this.studentService.GetStudentByIdAsync(model.StudentId);

            this.ViewBag.ClassId = student.ClassId;

            var subjects = await this.classService.GetAllSubjectsInClassDropdownAsync(student.ClassId);
            this.ViewBag.SubjectNames = subjects;

            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = student.Id;

            return this.View(model);
        }

        [Route("/Class/Students/RemarkDelete/{id}")]
        public async Task<IActionResult> RemarkDelete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var remark = await this.remarkService.GetRemarkByIdAsync(id);

            var result = await this.remarkService.DeleteRemarkAsync(id);

            if (result)
            {
                return this.RedirectToAction("RemarksAll", new { id = remark.StudentId });
            }

            return this.BadRequest();
        }

        [Route("/Class/Students/Grades/{id}")]
        public async Task<IActionResult> GradesAll(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var subjects = await this.studentService.GetStudentSubjectsAsync(id);

            var model = subjects.To<StudentGradesAllViewModel>().ToList();

            var student = await this.studentService.GetStudentByIdAsync(id);

            this.ViewBag.ClassId = student.ClassId;
            this.ViewBag.StudentName = student.FullName;
            this.ViewBag.StudentId = student.Id;

            return this.View(model);
        }

        [Route("/Class/Students/Grades/Subject/{id}")]
        public async Task<IActionResult> GradesPerSubject(string id, string studentId, int? pageNumber)
        {
            if (id == null || studentId == null)
            {
                return this.BadRequest();
            }

            var models = this.gradeService.GetGradesForSubject(studentId, id);

            var grades = models.To<GradeAllViewModel>();

            var student = await this.studentService.GetStudentByIdAsync(studentId);

            this.ViewBag.StudentId = studentId;
            this.ViewBag.SubjectId = id;

            this.ViewBag.StudentName = student.FullName;

            int pageSize = 8;

            return this.View(await PaginatedList<GradeAllViewModel>.CreateAsync(grades, pageNumber ?? 1, pageSize));
        }

        [Route("/Class/Students/Grades/Create/{id}")]
        public async Task<IActionResult> GradeCreate(string id, string studentId)
        {
            if (id == null || studentId == null)
            {
                return this.BadRequest();
            }

            var student = await this.studentService.GetStudentByIdAsync(studentId);
            this.ViewBag.StudentId = studentId;
            this.ViewBag.StudentName = student.FullName;

            var subject = await this.classService.GetSubjectById(id);
            this.ViewBag.SubjectId = id;
            this.ViewBag.SubjectName = subject.Subject.Name;

            return this.View();
        }

        [HttpPost]
        [Route("/Class/Students/Grades/Create/{id}")]
        public async Task<IActionResult> GradeCreate(GradeCreateInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<GradeServiceModel>();

                var result = await this.gradeService.CreateGradeAsync(serviceModel);

                var studentId = serviceModel.StudentId;
                var subjectId = serviceModel.SubjectId;

                var subject = await this.classService.GetSubjectById(subjectId);
                var student = await this.studentService.GetStudentByIdAsync(studentId);

                if (result)
                {
                    return this.RedirectToAction("GradesPerSubject", new { id = model.SubjectId, studentId = model.StudentId });
                }

                this.ViewBag.StudentId = studentId;
                this.ViewBag.SubjectId = subjectId;

                this.ViewBag.SubjectName = subject.Subject.Name;
                this.ViewBag.StudentName = student.FullName;

                return this.View(model);
            }

            return this.View(model);
        }

        [Route("/Class/Students/Grades/Edit/{id}")]
        public async Task<IActionResult> GradeEdit(string id)
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
        [Route("/Class/Students/Grades/Edit/{id}")]
        public async Task<IActionResult> GradeEdit(GradeEditInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var serviceModel = model.To<GradeServiceModel>();

                var result = await this.gradeService.EditGradeAsync(serviceModel);

                var studentId = model.StudentId;
                var subjectId = model.SubjectId;

                var subject = await this.classService.GetSubjectById(subjectId);
                var student = await this.studentService.GetStudentByIdAsync(studentId);

                if (result)
                {
                    return this.RedirectToAction("GradesPerSubject", new { id = model.SubjectId, studentId = model.StudentId });
                }

                this.ViewBag.StudentId = studentId;
                this.ViewBag.SubjectId = subjectId;

                this.ViewBag.SubjectName = subject.Subject.Name;
                this.ViewBag.StudentName = student.FullName;

                return this.View(model);
            }

            return this.View(model);
        }

        [Route("/Class/Students/Grades/Delete/{id}")]
        public async Task<IActionResult> GradeDelete(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var serviceModel = await this.gradeService.GetGradeByIdAsync(id);

            var result = await this.gradeService.DeleteGradeAsync(id);

            if (result)
            {
                return this.RedirectToAction("GradesPerSubject", new { id = serviceModel.SubjectId, studentId = serviceModel.StudentId });
            }

            return this.BadRequest();
        }
    }
}
