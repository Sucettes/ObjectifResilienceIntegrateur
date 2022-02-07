using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Stores
{
    public class RoleStore : RoleStore<Role, GwenaelDbContext, Guid, UserRole, RoleClaim>
    {
        public RoleStore(GwenaelDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}