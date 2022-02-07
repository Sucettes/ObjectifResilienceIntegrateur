using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Stores
{
    public class UserStore : UserStore<User, Role, GwenaelDbContext, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, RoleClaim>
    {
        public UserStore(GwenaelDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
