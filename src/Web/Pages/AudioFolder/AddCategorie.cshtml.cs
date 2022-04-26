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
    public class AddCategorieModel : PageModel
    {
        private readonly GwenaelDbContext _context;
        public IList<NewPage> NewPages { get; set; }


        public AddCategorieModel(GwenaelDbContext context)
        {
            _context = context;
        }
        //public IList<Article> Articles { get; set; }
        //public Article article { get; set; }

        public async Task<IActionResult> OnPostAsync( string nom, string description)
        {
            NewPages = await _context.NewPages.ToListAsync();
            CategoriesTutos cat = new CategoriesTutos
            {
                Nom = nom,
                Description = description
            };

            ViewData["NewPages"] = NewPages;

            _context.CategoriesTutos.Add(cat);
            await _context.SaveChangesAsync();
            return Page();
        }

    }
}
