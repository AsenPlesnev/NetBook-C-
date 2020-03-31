namespace NetBook.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class NetbookUserRole : IdentityUserRole<string>
    {
        public virtual NetBookUser User { get; set; }

        public virtual NetBookRole Role { get; set; }
    }
}
