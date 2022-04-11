using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Gwenael.Domain;

namespace Gwenael.Web.Pages
{
    public class ArticleIndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _db;

        public class InputModel
        {
            public List<Domain.Entities.Article> lstArticles { get; set; }

        }
        public ArticleIndexModel(GwenaelDbContext pDb) => _db = pDb;

        public IActionResult OnGet()
        {
            Input = new InputModel();
            Input.lstArticles = _db.Articles.ToList<Domain.Entities.Article>();

            return Page();
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectCreationArticle()
        {
            return RedirectToPage("CreationArticle");
        }



    }


}
