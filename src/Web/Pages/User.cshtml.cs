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
    public class UserModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public UserModel(GwenaelDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            user = await _context.Users.FindAsync(id);
            ViewData["user"] = user;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            //if (ModelState.IsValid)
            //{
            var userBd = await _context.Users.FindAsync(user.Id);
            userBd.UserName = user.UserName;
            userBd.Email = user.Email;
            userBd.LastName = user.LastName;
            userBd.FirstName = user.FirstName;

            await _context.SaveChangesAsync();
            return RedirectToPage("adminMenu");

            //}
        }

    }
}
