using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages
{
    public class NewPageCreation : PageModel
    {
        private readonly GwenaelDbContext _context;

        public NewPageCreation(GwenaelDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public NewPage newPage { get; set; }


        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPost(string titre, string inerText)
        {

            NewPage newPage = new NewPage
            {
                Titre = titre,
                InerText = inerText
            };

            _context.NewPages.Add(newPage);
            await _context.SaveChangesAsync();

            return Page();
        }

    }
}