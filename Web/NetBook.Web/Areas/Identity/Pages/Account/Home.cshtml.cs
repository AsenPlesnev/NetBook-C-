namespace NetBook.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using NetBook.Common;
    using NetBook.Services.Data.School;

#pragma warning disable SA1649 // File name should match first type name
    [Authorize(Roles = "Administrator")]
    public class HomeModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly ISchoolService schoolService;

        public HomeModel(ISchoolService schoolService)
        {
            this.schoolService = schoolService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var school = await this.schoolService.GetSchoolAsync();
            if (school == null)
            {
                return this.NotFound("Грешка при зараждането на училище.");
            }

            var schoolFullName = school.Name;
            var town = school.Town;
            var municipality = school.Municipality;
            var region = school.Region;
            var area = school.Area;

            this.Input = new InputModel
            {
                SchoolName = schoolFullName,
                Town = town,
                Municipality = municipality,
                Region = region,
                Area = area,
            };

            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Identity/Account/Login");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var school = await this.schoolService.GetSchoolAsync();
            if (school == null)
            {
                return this.NotFound("Грешка при зараждането на училище.");
            }

            var schoolName = school.Name;
            if (this.Input.SchoolName != schoolName)
            {
                school.Name = this.Input.SchoolName;

                await this.schoolService.UpdateAsync(school);
            }

            var town = school.Town;

            if (this.Input.Town != town)
            {
                school.Town = this.Input.Town;

                await this.schoolService.UpdateAsync(school);
            }

            var municipality = school.Municipality;

            if (this.Input.Municipality != municipality)
            {
                school.Municipality = this.Input.Municipality;

                await this.schoolService.UpdateAsync(school);
            }

            var region = school.Region;
            if (this.Input.Region != region)
            {
                school.Region = this.Input.Region;

                await this.schoolService.UpdateAsync(school);
            }

            var area = school.Area;
            if (this.Input.Area != area)
            {
                school.Area = this.Input.Area;

                await this.schoolService.UpdateAsync(school);
            }

            this.StatusMessage = "Информацията беше обновена!";
            return this.RedirectToPage();
        }

        public class InputModel
        {
            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.SchoolNameRegex, ErrorMessage = GlobalConstants.SchoolNameRegexError)]
            [MaxLength(GlobalConstants.SchoolNameLength)]
            public string SchoolName { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
            [MaxLength(GlobalConstants.CitizenshipLength)]
            public string Town { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
            [MaxLength(GlobalConstants.CitizenshipLength)]
            public string Municipality { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
            [MaxLength(GlobalConstants.CitizenshipLength)]
            public string Region { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
            [MaxLength(GlobalConstants.CitizenshipLength)]
            public string Area { get; set; }
        }
    }
}
