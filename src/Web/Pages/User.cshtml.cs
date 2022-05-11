using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Linq;

namespace Gwenael.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly GwenaelDbContext _context;
        private readonly UserManager<User> _userManager;

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
                return Page();
            }
            else
            {
                user = await _context.Users.FindAsync(id);
                ViewData["user"] = user;
                string erreur = Request.Query["erreur"];
                if (erreur != null)
                {
                    Console.WriteLine(erreur);
                    if (erreur == "courriel")
                    {
                        Console.WriteLine("Test");
                        ViewData["erreur"] = "Le courriel n'est pas valide";
                    }
                    else if (erreur == "courrielExist")
                    {
                        ViewData["erreur"] = "Le courriel est déjà utilisé";
                    }
                }
                return Page();
            }

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd, string ConfPassword, string NewPassword)
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
                if (userBd.Email != user.Email)
                {
                    if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        return Redirect("/User/" + user.Id + "/?erreur=courriel");
                    }
                    else
                    {
                        List<User> userTest = _context.Users.Where(u => u.Email == user.Email).ToList();
                        if (userTest.Count == 0)
                        {
                            userBd.Email = user.Email;
                            userBd.UserName = user.Email;
                        }
                        else
                        {
                            return Redirect("/User/" + user.Id +"/?erreur=courrielExist");
                        }
                    }
                }
                if (userBd.FirstName != user.FirstName)
                {
                    userBd.FirstName = user.FirstName;
                }
                if (userBd.LastName != user.LastName)
                {
                    userBd.LastName = user.LastName;
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
                bool mdpSontPareille = NewPassword == ConfPassword;
                if (NewPassword is not null && ConfPassword != null)
                {
                    if (mdpSontPareille)
                    {
                        User newUser = new User
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            UserName = user.Email,
                            Active = true
                        };
                        var result = await _userManager.CreateAsync(newUser, NewPassword);
                        if (result.Succeeded)
                        {
                            _context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine(result);
                        }

                    }
                }
                return RedirectToPage("adminMenu");
            }
            return RedirectToPage("index");
        }

    }
}
