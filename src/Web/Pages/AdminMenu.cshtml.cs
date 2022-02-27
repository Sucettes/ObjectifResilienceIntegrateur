using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gwenael.Web.Pages
{
    public class AdminMenuModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public AdminMenuModel(GwenaelDbContext context)
        {
            _context = context;
        }
        public IList<User> Users { get; set; }
        public IList<Formation> Formations { get; set; }
        public String Tab { get; set; }
        [BindProperty]
        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Query.Count == 1)
            {
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
            }

            Users = await _context.Users.ToListAsync();
            ViewData["lstUsers"] = Users;
            Formations = await _context.Formations.ToListAsync();
            ViewData["lstFormations"] = Formations;
            return Page();
        }
       
    }
}
