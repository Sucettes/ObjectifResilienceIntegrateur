using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;


namespace Gwenael.Web.Pages
{
    public class PoadcastDetailModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public PoadcastDetailModel(GwenaelDbContext context)
        {
            _context = context;
        }
        public IList<Article> Articles { get; set; }
        public Article article { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            Audio audio = _context.Audios.Find(id);
            ViewData["Audio"] = audio;
            return Page();
        }

    }
}
