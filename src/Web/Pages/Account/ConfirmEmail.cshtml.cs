using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gwenael.Domain.Entities;
using System.Collections.Generic;
using Gwenael.Domain;
using Microsoft.EntityFrameworkCore;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public IList<NewPage> NewPages { get; set; }
        private readonly GwenaelDbContext _db;

        public ConfirmEmailModel(UserManager<User> userManager, GwenaelDbContext pDb)
        {
            _userManager = userManager;
            _db = pDb;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            NewPages = await _db.NewPages.ToListAsync();
            ViewData["NewPages"] = NewPages;

            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                if (Permission.EstAdministrateur(idConnectedUser, _db)) {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }

                    if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Error confirming email for user with ID '{userId}':");
            }

            return Page();
        }
    }
}
