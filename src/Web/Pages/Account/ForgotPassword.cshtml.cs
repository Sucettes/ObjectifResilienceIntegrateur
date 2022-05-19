using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gwenael.Application.Mailing;
using Gwenael.Domain.Entities;
using Gwenael.Web.Extensions;
using System.Collections.Generic;
using Gwenael.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        public IList<NewPage> NewPages { get; set; }
        private readonly GwenaelDbContext _db;

        private readonly UserManager<User> _userManager;
        private readonly IEmailFactory _emailFactory;

        public ForgotPasswordModel(UserManager<User> userManager, IEmailFactory emailFactory,
        GwenaelDbContext pDb)
        {
            _userManager = userManager;
            _emailFactory = emailFactory;
            _db = pDb;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                if (Permission.EstAdministrateur(idConnectedUser, _db))
                {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }


            NewPages = await _db.NewPages.ToListAsync();
            ViewData["NewPages"] = NewPages;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailFactory.SendResetPasswordAsync(Input.Email, user.UserName, callbackUrl);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
