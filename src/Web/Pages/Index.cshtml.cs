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
    public class IndexModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public IndexModel(GwenaelDbContext context)
        {
            _context = context;
        }
        public IList<Article> Articles { get; set; }
        public Article article { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Articles = await _context.Articles.ToListAsync();

            ViewData["lstArticles"] = Articles;
            return Page();
        }

    }
}
