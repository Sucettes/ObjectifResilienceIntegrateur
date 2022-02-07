using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spk.Common.Helpers.Guard;
using Gwenael.Application.Dtos;
using Gwenael.Application.Mailing;
using Gwenael.Application.Services;
using Gwenael.Application.Settings;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using Gwenael.Web.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Areas.Api
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class AccountController : SecuredApiControllerBase
    {
        private readonly IEmailFactory _emailFactory;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly MailingSettings _mailingSettings;

        public AccountController(GwenaelUserManager userManager, IEmailFactory emailFactory, IUserService userService, IMapper mapper, IOptions<MailingSettings> options) : base(userManager)
        {
            _emailFactory = emailFactory.GuardIsNotNull(nameof(emailFactory));
            _userService = userService.GuardIsNotNull(nameof(userService));
            _mapper = mapper.GuardIsNotNull(nameof(mapper));
            _mailingSettings = options.GuardIsNotNull(nameof(options)).Value;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account")]
        public async Task<IActionResult> Get()
        {
            if (User.Identity == null) 
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            return new JsonResult(_mapper.Map<AccountDto>(user));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account")]
        [Authorize(Policy = "Permissions.User.Create")]
        public async Task<IActionResult> Post([FromBody] UserDto data)
        {
            var user = new User();
            _mapper.Map(data, user);

            var errors = user.Validate();
            if (errors.Any())
            {
                return ReturnBadRequestWithErrors(errors);
            }

            var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());
            if (!result.Succeeded)
            {
                return ReturnBadRequestWithErrors(result.Errors.ToArray());
            }

            // --------------------
            // Add user to roles
            // --------------------
            if (data.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, data.Roles);

                if (!roleResult.Succeeded)
                {
                    return ReturnBadRequestWithErrors(roleResult.Errors.ToArray());
                }
            }


            if (user.Active)
            {
                // Send reset password email
                var referrer = HttpContext.Request.Headers["Referer"].ToString();
                await SendResetPassword(user, referrer);
            }

            return new JsonResult(_mapper.Map<AccountDto>(user));
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account/{userName}")]
        [Authorize(Policy = "Permissions.User.Edit")]
        public async Task<IActionResult> Put(string userName, [FromBody] UserDto data)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (userName != user.UserName)
            {
                return BadRequest();
            }

            _mapper.Map(data, user);

            var errors = user.Validate();
            if (errors.Any())
            {
                return ReturnBadRequestWithErrors(errors);
            }

            // Clear Roles from User object
            user.Updated();
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ReturnBadRequestWithErrors(result.Errors.ToArray());
            }

            // --------------------
            // Add user to roles
            // --------------------
            if (data.Roles.Any())
            {
                var activeUserRoles = await _userManager.GetRolesAsync(user);
                var roleResult =
                    await _userManager.AddToRolesAsync(user, data.Roles.Where(r => !activeUserRoles.Contains(r)));

                if (!roleResult.Succeeded)
                {
                    return ReturnBadRequestWithErrors(roleResult.Errors.ToArray());
                }

                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user,
                    activeUserRoles.Where(activeRole => data.Roles.All(r => r != activeRole)));

                if (!removeRolesResult.Succeeded)
                {
                    return ReturnBadRequestWithErrors(result.Errors.ToArray());
                }
            }

            return new JsonResult(_mapper.Map<AccountDto>(user));
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account/{userName}/password")]
        [Authorize(Policy = "Permissions.User.Edit")]
        public async Task<IActionResult> Put(string userName, [FromBody] PasswordDto data)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            user.Updated();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, data.Password);

            if (!result.Succeeded)
            {
                return ReturnBadRequestWithErrors(result.Errors.ToArray());
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account/forgotpassword")]
        public async Task<IActionResult> Post([FromBody] ForgotPasswordDto data)
        {

            var user = await _userManager.FindByNameAsync(data.UserName);

            if (user == null)
                return NotFound();

            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            await SendResetPassword(user, referrer);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiVersion("1.0")]
        [Route("v{version:apiVersion}/account/resetpassword")]
        public async Task<IActionResult> Post([FromBody] ResetPasswordDto data)
        {
            var user = await _userManager.FindByIdAsync(data.Id);

            if (user == null)
                return NotFound();

            if (user.UserName != data.UserName)
                return BadRequest();

            user.Updated();
            var result = await _userManager.ResetPasswordAsync(user, data.Code, data.NewPassword);

            if (!result.Succeeded)
                return ReturnBadRequestWithErrors(result.Errors.ToArray());

            return Ok();
        }

        #region Private

        private async Task SendResetPassword(User user, string referrer)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, token, Request.Scheme, referrer);

            await _emailFactory.SendResetPasswordAsync(user.Email, user.UserName, callbackUrl, _mailingSettings.SupportAddress);
        }


        #endregion
    }
}
