using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Stores
{
    public class GwenaelRoleManager : RoleManager<Role>
    {
        public GwenaelRoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        public async Task<Role> FindByIdAsync(Guid roleId)
        {
            return await Roles
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Id == roleId);
        }

        public override async Task<Role> FindByNameAsync(string roleName)
        {
            return await Roles
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(x => x.Name == roleName);
        }
    }
}