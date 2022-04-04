using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using Microsoft.AspNetCore.Identity;

namespace Gwenael.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly GwenaelDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly PasswordHasher<User> _passHasher = new PasswordHasher<User>();


        public UserModel(GwenaelDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (Guid.Empty == id)
            {
                Console.WriteLine("Yo big");
                return Page();
            }
            else
            {
                user = await _context.Users.FindAsync(id);
                ViewData["user"] = user;
                ViewData["passwordUser"] = user.PasswordHash;
                return Page();
            }

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd, string NewPassword)
        {
            User userBd = null;
            try
            {
                userBd = await _context.Users.FindAsync(user.Id);
            }
            catch
            {
            }
            if (btnSave != null)
            {
                userBd.UserName = user.UserName;
                userBd.Email = user.Email;
                userBd.LastName = user.LastName;
                userBd.FirstName = user.FirstName;
                if (NewPassword is not null)
                {
                    string hashedPassword = _passHasher.HashPassword(userBd, NewPassword);
                    userBd.PasswordHash = hashedPassword;

                }
                await _context.SaveChangesAsync();
                return RedirectToPage("adminMenu");
            }
            else if (btnDelete != null)
            {
                _context.Users.Remove(userBd);
                _context.SaveChanges();
                return RedirectToPage("adminMenu");
            }
            else if (btnAdd != null)
            {
                if (NewPassword is not null)
                {
                    string hashedPassword = _passHasher.HashPassword(userBd, NewPassword);
                    user.PasswordHash = hashedPassword;
                }
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash

                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToPage("adminMenu");
            }
            return RedirectToPage("index");
        }

    }
}
