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
        //public IList<Poadcast> poadcasts { get; set; }
        public IList<Audio> audios { get; set; }
        public IList<Audio> lstAudioSort { get; set; }
        public String Categorie { get; set; }
        //Poadcast poadcast { get; set; }
        public IList<Audio> audio { get; set; }
        [BindProperty(SupportsGet = true)]
        public string cat { get; set; }

        //public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Query.Count == 1)
            {
                Categorie = Request.Query["catégorie"];
                ViewData["Catégorie"] = Categorie;
            }
            audios = await _context.Audios.ToListAsync();
            ViewData["lstAudios"] = audios;
            //var poadcastSort = poadcasts;
            
            var poadcastSort = from p in _context.Audios select p;
            if (cat == null)
            {
                if (Request.Query.Count == 1)
                {
                    Categorie = Request.Query["catégorie"];
                    ViewData["Catégorie"] = Categorie;
                }
                audios = await _context.Audios.ToListAsync();
                ViewData["lstPoadcasts"] = audios;
            }
            if(cat =="lowtech" || cat == "recyclage" || cat =="autCat")
            {
                foreach (var p in ViewData["lstPoadcasts"] as IList<Audio>)
                {
                    //poadcastSort = poadcastSort.Where(p => p.categorie == cat);
                    //lstPoadcastSort = await poadcastSort.ToListAsync();
                    //lstPoadcastSort.Add((Poadcast)poadcastSort);
                }
                ViewData["lstPoadcasts"] = lstAudioSort;
            }
            else
            {
                if (Request.Query.Count == 1)
                {
                    Categorie = Request.Query["catégorie"];
                    ViewData["Catégorie"] = Categorie;
                }
                audios = await _context.Audios.ToListAsync();
                ViewData["lstAudios"] = audios;
            }
           
            return Page();
        }

        
    }
}
