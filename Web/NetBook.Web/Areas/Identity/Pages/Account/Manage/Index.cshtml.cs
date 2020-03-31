namespace NetBook.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using NetBook.Common;
    using NetBook.Data.Models;

#pragma warning disable SA1649 // File name should match first type name
    [Authorize]
    public class IndexModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<NetBookUser> userManager;
        private readonly SignInManager<NetBookUser> signInManager;
        private readonly IEmailSender emailSender;

        public IndexModel(
            UserManager<NetBookUser> userManager,
            SignInManager<NetBookUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            NetBookUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Грешка при намиране на потребителя '{user.FullName}'.");
            }

            string userName = this.User.FindFirst("FullName").Value;
            string email = await this.userManager.GetEmailAsync(user);
            string phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            NetBookUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Грешка при намиране на потребителя '{user.FullName}'.");
            }

            string email = await this.userManager.GetEmailAsync(user);
            if (this.Input.Email != email)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, this.Input.Email);
                var setUserNameResult = await this.userManager.SetUserNameAsync(user, this.Input.Email);

                if (!setEmailResult.Succeeded || !setUserNameResult.Succeeded)
                {
                    string userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Възникна неочаквана грешка при смяната на електронната поща/името.");
                }
            }

            string phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException($"Възникна неочаквана грешка при смяната на телефона на потребителя {user.FullName}.");
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Промените се запазиха!";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            NetBookUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Грешка при намиране на потребителя '{user.FullName}'.");
            }

            string userId = await this.userManager.GetUserIdAsync(user);
            string email = await this.userManager.GetEmailAsync(user);
            string code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            string callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: this.Request.Scheme);
            await this.emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.StatusMessage = "Verification email sent. Please check your email.";
            return this.RedirectToPage();
        }

        public class InputModel
        {
            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [EmailAddress(ErrorMessage = GlobalConstants.EmailErrorMessage)]
            public string Email { get; set; }

            [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
            [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PhoneRegexError)]
            [MaxLength(GlobalConstants.PhoneLength)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
