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
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly GwenaelDbContext _context;
        private readonly UserManager<User> _userManager;
        private IList<NewPage> NewPages { get; set; }


        public UserModel(GwenaelDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _context);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _context);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {
                    NewPages = await _context.NewPages.ToListAsync();
                    ViewData["NewPages"] = NewPages;

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
            }
            return RedirectToPage("index");

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd, string ConfPassword, string NewPassword)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _context);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {
                    NewPages = await _context.NewPages.ToListAsync();
                    ViewData["NewPages"] = NewPages;

                    User userBd = null;
                    try
                    {
                        userBd = await _context.Users.FindAsync(user.Id);
                    }
                    catch
                    {
                    }
                    Console.WriteLine(userBd);
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
                                try
                                {
                                    List<User> userTest = _context.Users.Where(u => u.Email == user.Email).ToList();
                                    if (userTest.Count == 0)
                                    {
                                        userBd.Email = user.Email.ToString();
                                        userBd.UserName = user.Email.ToString();
                                    }
                                    else
                                    {
                                        return Redirect("/User/" + user.Id + "/?erreur=courrielExist");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Erreur");
                                }

                            }
                        }
                        if (user.FirstName.Length > 0)
                        {
                            userBd.FirstName = user.FirstName.ToString();
                        }
                        else
                        {
                            userBd.FirstName = "Non défini";
                        }
                        if (user.LastName.Length > 0)
                        {
                            userBd.LastName = user.LastName.ToString();
                        }
                        else
                        {
                            userBd.LastName = "Non défini";

                        }
                        _context.SaveChanges();
                        return Redirect("/AdminMenu/?tab=utilisateurs");
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
            return RedirectToPage("index");
        }

    }
}
