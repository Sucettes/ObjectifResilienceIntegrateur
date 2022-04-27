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

        

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                audios = await _context.Audios.ToListAsync();

                ViewData["lstAudios"] = audios;

                lstCategories = _context.CategoriesTutos.ToList();

                return Page();
            }
            catch
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPost(string categorie)
        {
            try
            {
                audios = await _context.Audios.ToListAsync();
                ViewData["lstAudios"] = audios;

                if (categorie != "tout" && categorie != null)
                {

                    lstCategories = _context.CategoriesTutos.ToList();

                    var audioSort = from p in _context.Audios select p;
                    CategoriesTutos cat = _context.CategoriesTutos.Where(c => c.Nom == categorie).First();

                    foreach (var p in ViewData["lstAudios"] as IList<Audio>)
                    {
                        audioSort = audioSort.Where(p => p.categorie == cat);
                        lstAudioSort = await audioSort.ToListAsync();
                    }
                    ViewData["lstAudios"] = lstAudioSort;

                }
                else
                {
                    audios = await _context.Audios.ToListAsync();
                    ViewData["lstAudios"] = audios;
                }
                lstCategories = _context.CategoriesTutos.ToList();
                return Page();
            }
            catch
            {
                return Page();
            }

        }    
    }
}
