using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public String Tab { get; set; }
        Poadcast poadcast { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //if (Request.Query.Count == 1)
            //{
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
            //}
            poadcasts = await _context.Poadcasts.ToListAsync();
            ViewData["lstPoadcasts"] = poadcasts;
            return Page();
        }
    }
}
