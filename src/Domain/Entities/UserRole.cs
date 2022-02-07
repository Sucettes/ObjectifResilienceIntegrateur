using System;
using Microsoft.AspNetCore.Identity;

namespace Gwenael.Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

        public UserRole()
        {
            
        }

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
