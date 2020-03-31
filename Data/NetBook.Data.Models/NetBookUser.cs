// ReSharper disable VirtualMemberCallInConstructor

namespace NetBook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using NetBook.Data.Common.Models;


    public class NetBookUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public NetBookUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<NetbookUserRole>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        public string FullName { get; set; }

        public bool IsTeacher { get; set; }

        public bool IsClassTeacher { get; set; }

        public string ClassId { get; set; }

        public Class Class { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<NetbookUserRole> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    }
}
