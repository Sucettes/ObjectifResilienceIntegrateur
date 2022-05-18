using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Web.FctUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gwenael.Web.Pages.Account
{
    public class SignedOutModel : PageModel
    {
        public IList<NewPage> NewPages { get; set; }
        private readonly GwenaelDbContext _db;

        public SignedOutModel(GwenaelDbContext pDb)
        {
            _db = pDb;
        }

        public IActionResult OnGet()
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

            NewPages = _db.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}