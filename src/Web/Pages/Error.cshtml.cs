using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Web.FctUtils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gwenael.Web.Pages
{
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        private IList<NewPage> NewPages { get; set; }
        private readonly GwenaelDbContext _context;

        public ErrorModel(GwenaelDbContext context)
        {
            _context = context;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _context);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }


            NewPages = _context.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
