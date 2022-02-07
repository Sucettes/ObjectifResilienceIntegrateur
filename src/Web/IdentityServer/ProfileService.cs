using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Gwenael.Domain.Entities;

namespace Gwenael.Web.IdentityServer
{
    public sealed class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ProfileService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subject);
            var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            var claims = userClaims.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            if (_userManager.SupportsUserRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                    if (_roleManager.SupportsRoleClaims)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            claims.AddRange(await _roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subject);
            context.IsActive = user != null;
        }
    }
}