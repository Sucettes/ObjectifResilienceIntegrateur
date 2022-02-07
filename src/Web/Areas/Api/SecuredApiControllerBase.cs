using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spk.Common.Helpers.Guard;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Areas.Api
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Area(AreaNames.Api)]
    [Consumes("application/json")]
    public class SecuredApiControllerBase : ControllerBase
    {
        protected readonly GwenaelUserManager _userManager;

        protected SecuredApiControllerBase(GwenaelUserManager userManager)
        {
            _userManager = userManager.GuardIsNotNull(nameof(userManager));
        }

        protected async Task<User> CurrentUser()
        {
            return await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        protected async Task<bool> IsSuperAdmin()
        {
            return await _userManager.IsInRoleAsync(await CurrentUser(), Domain.Constants.Authorization.Roles.SUPER_ADMIN);
        }

        protected IActionResult ReturnBadRequestWithErrors(params string[] errors)
        {
            var validationResult = new ValidationResult();
            validationResult.Errors
                .AddRange(errors.Select(e =>
                        new ValidationFailure(string.Empty, e)
                    )
                );

            return this.ReturnBadRequestWithErrors(validationResult);
        }

        protected IActionResult ReturnBadRequestWithErrors(params IdentityError[] errors)
        {
            var validationResult = new ValidationResult();
            validationResult.Errors
                .AddRange(errors.Select(e =>
                        new ValidationFailure(string.Empty, e.Code)
                    )
                );

            return this.ReturnBadRequestWithErrors(validationResult);
        }
        protected IActionResult ReturnBadRequestWithErrors(ValidationResult result)
        {
            result.AddToModelState(ModelState, null);

            var formattedModelErrors = ModelState.ToDictionary(entry =>
                    entry.Key,
                entry => entry.Value.Errors.Select(error =>
                    (object)new
                    {
                        Code = error.ErrorMessage?.Length > 100
                            ? "Exception"
                            : error.ErrorMessage,
                        error.Exception?.Message
                    }));

            //Logger.LogWarning(
            //    "ModelState is invalid in API action : Errors are {modelStateErrors}",
            //    JsonConvert.SerializeObject(formattedModelErrors));

            return BadRequest(new
            {
                isSuccess = false,
                errors = formattedModelErrors
            });
        }
    }
}
