using Microsoft.AspNetCore.Identity;
using Gwenael.Domain;
using Gwenael.Domain.Authorization;
using Gwenael.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gwenael.Web.Extensions
{
    public static class RoleManagerExtensions
    {
        public static async Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Constants.Authorization.Roles.SUPER_ADMIN);
            await roleManager.AddNewPermissionClaim(adminRole, Permissions.Generate());
        }

        public static async Task AddNewPermissionClaim(this RoleManager<Role> roleManager, Role role,
            IEnumerable<string> permissionsToAdd)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in permissionsToAdd)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }

        public static async Task RemoveDeprecatedPermissionClaim(this RoleManager<Role> roleManager, Role role, IEnumerable<string> permissionsToKeep)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in allClaims.Where(x => x.Type == "Permission"))
            {
                if (permissionsToKeep.All(a => a != claim.Value))
                {
                    await roleManager.RemoveClaimAsync(role, claim);
                }
            }
        }

        public static async Task SyncPermissionClaim(this RoleManager<Role> roleManager, Role role,
            IEnumerable<string> permissions)
        {
            await AddNewPermissionClaim(roleManager, role, permissions);
            await RemoveDeprecatedPermissionClaim(roleManager, role, permissions);
        }
    }
}