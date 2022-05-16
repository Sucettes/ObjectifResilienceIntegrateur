using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class PoadcastPageModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public PoadcastPageModel(GwenaelDbContext context)
        {
            _context = context;
        }
        
        public IList<Audio> audios { get; set; }
        public IList<Audio> lstAudioSort { get; set; }
        public List<CategoriesTutos> lstCategories { get; set; }
        public String Categorie { get; set; }
        
        public IList<Audio> audio { get; set; }
        [BindProperty(SupportsGet = true)]
        public string cat { get; set; }
        public string descriptionCoupe { get; set; }
        public string recherche { get; set; }
        public bool droitAccess { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                audios = await _context.Audios.ToListAsync();
                ViewData["lstAudios"] = audios;
                recherche = Request.Query["recherche"];
                
                if (recherche != null)
                {
                    lstCategories = _context.CategoriesTutos.ToList();

                    var audioSort = from p in _context.Audios select p;
                    CategoriesTutos cat = _context.CategoriesTutos.Where(c => c.Nom == recherche).First();

                    foreach (var p in ViewData["lstAudios"] as IList<Audio>)
                    {
                        audioSort = audioSort.Where(p => p.categorie == cat);
                        lstAudioSort = await audioSort.ToListAsync();
                    }
                    ViewData["lstAudios"] = lstAudioSort;
                    ViewData["recherche"] = recherche;
                }
                else
                {
                    audios = await _context.Audios.ToListAsync();

                    ViewData["lstAudios"] = audios;
                }

                lstCategories = _context.CategoriesTutos.ToList();

                droitAccess = false;
                if (User.Identity.IsAuthenticated)
                {
                    Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
                    if (Permission.VerifierAccesGdC(idConnectedUser, _context))
                    {
                        droitAccess = true;
                    }
                }

                return Page();
            }
            catch
            {
                return Page();
            }
        }

        public Guid ObtenirIdDuUserSelonEmail(string email)
        {
            User user = (User)_context.Users.Where(u => u.UserName == email).First();
            return user.Id;
        }

        public async Task<IActionResult> OnPost(string categorie)
        {
           
                audios = await _context.Audios.ToListAsync();
                ViewData["lstAudios"] = audios;


            if(categorie != null)
            {
                return Redirect("/AudioFolder/PoadcastPage?recherche=" + categorie);
            }
            else
            {
                return Redirect("/AudioFolder/PoadcastPage");
            }
            

        }    
    }
}
