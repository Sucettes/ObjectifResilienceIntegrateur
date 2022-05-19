using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using System.Linq;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using System.IO;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class NewPagesModel : PageModel
    {
        private readonly GwenaelDbContext _context;


        public NewPagesModel(GwenaelDbContext context)
        {
            _context = context;
        }
        public List<NewPage> NewPages { get; set; }
        public NewPage newPage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            NewPages = await _context.NewPages.ToListAsync();
            string titre = Request.Query["Titre"];

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

            newPage = NewPages.Where(newPage => newPage.Titre == titre).First();
            ViewData["newPage"] = newPage;
            ViewData["NewPages"] = NewPages;


            return Page();
        }

    }
}