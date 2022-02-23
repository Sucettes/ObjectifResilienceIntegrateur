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
            if (id.ToString() is null or "")
            {
                return Page();
            }
            else
            {
                user = await _context.Users.FindAsync(id);
                ViewData["user"] = user;
                return Page();
            }

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd)
        {
            User userBd = null;
            try
            {
                userBd = await _context.Users.FindAsync(user.Id);
            }
            catch
            {
            }
            //if (ModelState.IsValid)
            //{
            //}
            if (btnSave != null)
            {
                userBd.UserName = user.UserName;
                userBd.Email = user.Email;
                userBd.LastName = user.LastName;
                userBd.FirstName = user.FirstName;

                await _context.SaveChangesAsync();
                return RedirectToPage("adminMenu");
            }
            else if (btnDelete != null)
            {
                _context.Users.Remove(userBd);
                await _context.SaveChangesAsync();
                return RedirectToPage("adminMenu");
            }
            else if (btnAdd != null)
            {
                //Ajout Manuel d'un user
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName
                };
                _context.Add<User>(newUser);
                _context.SaveChanges();
                return RedirectToPage("adminMenu");
            }
            return RedirectToPage("index");



        }

    }
}
