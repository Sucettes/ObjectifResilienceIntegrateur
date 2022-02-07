using System;
using Microsoft.AspNetCore.Identity;

namespace Gwenael.Domain.Entities
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public RoleClaim()
        {
            
        }

        public RoleClaim(Guid roleId, string type, string value)
        {
            RoleId = roleId;
            ClaimType = type;
            ClaimValue = value;
        }
        public override string ToString()
        {
            return ClaimValue;
        }
    }
}
