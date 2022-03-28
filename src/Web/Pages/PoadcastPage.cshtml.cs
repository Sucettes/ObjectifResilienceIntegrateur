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
        public IList<Poadcast> poadcasts { get; set; }
        public IList<Poadcast> lstPoadcastSort { get; set; }
        public String Categorie { get; set; }
        Poadcast poadcast { get; set; }

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
            poadcasts = await _context.Poadcasts.ToListAsync();
            ViewData["lstPoadcasts"] = poadcasts;
            //var poadcastSort = poadcasts;
            //Categories = new SelectList(_context.Set<Ca>)
            var poadcastSort = from p in _context.Poadcasts select p;
            if (cat == null)
            {
                if (Request.Query.Count == 1)
                {
                    Categorie = Request.Query["catégorie"];
                    ViewData["Catégorie"] = Categorie;
                }
                poadcasts = await _context.Poadcasts.ToListAsync();
                ViewData["lstPoadcasts"] = poadcasts;
            }
            if(cat =="lowtech" || cat == "recyclage" || cat =="autCat")
            {
                //foreach (var p in ViewData["lstPoadcasts"] as IList<Poadcast>)
                //{
                    poadcastSort = poadcastSort.Where(p => p.categorie == cat);
                    lstPoadcastSort = await poadcastSort.ToListAsync();
                //    //lstPoadcastSort.Add((Poadcast)poadcastSort);
                //}
                ViewData["lstPoadcasts"] = lstPoadcastSort;
            }
            else
            {
                if (Request.Query.Count == 1)
                {
                    Categorie = Request.Query["catégorie"];
                    ViewData["Catégorie"] = Categorie;
                }
                poadcasts = await _context.Poadcasts.ToListAsync();
                ViewData["lstPoadcasts"] = poadcasts;
            }
            

                //var poacastSort = _context.Poadcasts.Where()

            

            //foreach (var poadcast in ViewData["lstPoadcasts"] as IList<Poadcast>)
            //{
            //    if (cat == "lowtech")
            //    {

            //    }
            //}

            return Page();
        }

        
    }
}
