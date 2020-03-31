namespace NetBook.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using NetBook.Services.Data.School;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Mapping;
    using NetBook.Web.InputModels.Home;
    using NetBook.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IStudentService studentService;
        private readonly ISchoolService schoolService;

        public HomeController(IStudentService studentService, ISchoolService schoolService)
        {
            this.studentService = studentService;
            this.schoolService = schoolService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string url = "/Identity/Account/Manage";

                return this.LocalRedirect(url);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var pin = model.PIN;

                var student = await this.studentService.GetStudentToDisplayAsync(pin);

                if (student == null)
                {
                    return this.RedirectToAction("Index");
                }

                student.School = await this.schoolService.GetSchoolAsync();

                var viewModel = student.To<DisplayStudentViewModel>();

                this.ViewBag.Index = 1;

                return this.View("DisplayStudent", viewModel);
            }

            return this.View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
