namespace NetBook.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using NetBook.Common;
    using NetBook.Data.Models;
    using NetBook.Services.Data.User;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<NetBookUser> signInManager;
        private readonly UserManager<NetBookUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;

        public RegisterModel(
            UserManager<NetBookUser> userManager,
            SignInManager<NetBookUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                returnUrl = "/Identity/Account/Home";

                return this.LocalRedirect(returnUrl);
            }

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.ModelState.IsValid && (this.Input.RegisterCode == GlobalConstants.RegisterTeacherCode || this.Input.RegisterCode == GlobalConstants.RegisterAdministratorCode))
            {
                var user = new NetBookUser { UserName = this.Input.Email, Email = this.Input.Email, FullName = this.Input.FullName, PhoneNumber = this.Input.PhoneNumber };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    if (this.Input.RegisterCode == GlobalConstants.RegisterTeacherCode)
                    {
                        var roleResult = await this.userManager.AddToRoleAsync(user, GlobalConstants.TeacherRoleName);

                        await this.userService.AddUserAsTeacherAsync(user.Id);
                    }
                    else if (this.Input.RegisterCode == GlobalConstants.RegisterAdministratorCode)
                    {
                        var roleResult =
                            await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                    }

                    this.logger.LogInformation("User created a new account with password.");

                    returnUrl = "/Identity/Account/Home";

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [EmailAddress(ErrorMessage = GlobalConstants.EmailErrorMessage)]
            public string Email { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.FullNameRegexError)]
            [MaxLength(GlobalConstants.FullNameLength)]
            public string FullName { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PhoneRegexError)]
            [MaxLength(GlobalConstants.PhoneLength)]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [StringLength(100, ErrorMessage = GlobalConstants.PasswordLengthErrorMessage, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = GlobalConstants.ConfirmPasswordErrorMessage)]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            public string RegisterCode { get; set; }
        }
    }
}
