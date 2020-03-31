namespace NetBook.Data
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    using NetBook.Data.Models;


    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<NetBookUser, NetBookRole>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<NetBookUser> userManager,
            RoleManager<NetBookRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(NetBookUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullName", user.FullName ?? string.Empty));
            return identity;
        }
    }
}
