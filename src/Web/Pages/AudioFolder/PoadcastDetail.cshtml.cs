using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using System.Linq;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class PoadcastDetailModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public PoadcastDetailModel(GwenaelDbContext context)
        {
            _context = context;
        }
        public List<CategoriesTutos> lstCategories { get; set; }
        CategoriesTutos Cat { get; set; }
        public Article article { get; set; }
        public Audio audio { get; set; }
        public IList<NewPage> NewPages { get; set; }

        public IActionResult OnGet(Guid? id)
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

            try
            {
                NewPages = _context.NewPages.ToList();

                lstCategories = _context.CategoriesTutos.ToList();
                audio = _context.Audios.Find(id);
                ViewData["Audio"] = audio;
                Cat = _context.CategoriesTutos.Where(c => c.Id == audio.categorie.Id).First();
                ViewData["Cat"] = Cat;
                ViewData["NewPages"] = NewPages;

                return Page();

            }
            catch
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string name, string btnAjouterPoadcast, string btnSupprimerPoadcast, int? id)
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

            try
            {
                if (btnAjouterPoadcast is not null)
                {
                    Audio audioBd = _context.Audios.Where(u => u.ID == Guid.Parse(name)).First();
                    audioBd.EstPublier = true;
                    await _context.SaveChangesAsync();
                    return Redirect("/AdminMenu/?tab=podcasts");
                }
                else if (btnSupprimerPoadcast is not null)
                {
                    Audio audioBd = _context.Audios.Where(u => u.ID == Guid.Parse(name)).First();
                    _context.Audios.Remove(audioBd);
                    await _context.SaveChangesAsync();
                    return Redirect("/AdminMenu/?tab=podcasts");
                }
                return Page();
            }
            catch
            {
                return Page();
            }


        }
    }
        
}
