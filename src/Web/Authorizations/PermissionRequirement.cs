using Microsoft.AspNetCore.Authorization;

namespace Gwenael.Web.Authorizations
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}