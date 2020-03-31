// ReSharper disable VirtualMemberCallInConstructor

using System.Collections.Generic;

namespace NetBook.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;

    using NetBook.Data.Common.Models;

    public class NetBookRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public NetBookRole()
            : this(null)
        {
            this.UsersRoles = new HashSet<NetbookUserRole>();
        }

        public NetBookRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<NetbookUserRole> UsersRoles { get; set; }
    }
}
