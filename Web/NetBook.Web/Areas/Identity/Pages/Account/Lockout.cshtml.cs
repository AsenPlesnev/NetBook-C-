namespace NetBook.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class LockoutModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        public IActionResult OnGet()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var url = "/Identity/Account/Home";

                return this.LocalRedirect(url);
            }

            return this.Page();
        }
    }
}
