using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Gwenael.Web.Authorizations
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissions = context.User?.Claims.Where(x => x.Type == "Permission" &&
                                                              x.Value == requirement.Permission);

            if (permissions.Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}