namespace NetBook.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using NetBook.Common;
    using NetBook.Data.Models;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class LoginModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<NetBookUser> signInManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(SignInManager<NetBookUser> signInManager, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                returnUrl = "/Identity/Account/Manage";

                return this.LocalRedirect(returnUrl);
            }

            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("Потребителя влезна.");

                    returnUrl = "/Identity/Account/Manage";

                    return this.LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("Акаунта на потребителя беше заключен.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Грешна електронна поща или парола.");
                    return this.Page();
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
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
