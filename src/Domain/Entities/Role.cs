using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Gwenael.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public virtual IEnumerable<UserRole> Users { get; set; }

        public virtual IEnumerable<RoleClaim> Claims { get; set; }
        public bool Active { get; set; }
        public Role()
        {
            
        }
        public Role(string name, string[] permissionClaims, bool active = true)
        {
            Name = name;
            Active = active;
            Claims = permissionClaims?.Select(x => new RoleClaim(Id, "Permission", x)).ToList();
        }

        public IEnumerable<string> GetClaims()
        {
            return Claims.Select(x => x.ClaimValue);
        }
    }
}