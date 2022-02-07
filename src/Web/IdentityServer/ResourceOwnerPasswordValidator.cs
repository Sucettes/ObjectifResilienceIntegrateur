using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Gwenael.Web.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<User> _signInManager;
        private readonly GwenaelUserManager _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

        public ResourceOwnerPasswordValidator(
            GwenaelUserManager userManager,
            SignInManager<User> signInManager,
            ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);

                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    return;
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user_locked_out");
                    return;
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                }
                else
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                }
            }
            else
            {
                _logger.LogInformation("No user found matching username: {username}", context.UserName);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid_username_password");
        }
    }
}
