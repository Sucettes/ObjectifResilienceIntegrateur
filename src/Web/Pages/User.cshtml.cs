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
                return Page();
            }

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd, string NewPassword, string ConfPassword)
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
                //Console.WriteLine(NewPassword);
                //Console.WriteLine(ConfPassword);

                //if (NewPassword is not null)
                //{
                //    string hashedPassword = _passHasher.HashPassword(userBd, NewPassword);
                //    if (ConfPassword is not null)
                //    {
                //        PasswordVerificationResult result = _passHasher.VerifyHashedPassword(userBd, hashedPassword, ConfPassword);
                //        if (result is PasswordVerificationResult.Success)
                //        {
                //            Console.WriteLine("Success");
                //            userBd.PasswordHash = hashedPassword;
                //        }
                //        else
                //        {
                //            return Page();
                //        }
                //    }
                //}
                //userBd.UserName = user.UserName;
                //userBd.Email = user.Email;
                //userBd.LastName = user.LastName;
                //userBd.FirstName = user.FirstName;
                //await _context.SaveChangesAsync();
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
                            Console.WriteLine("Yo big c'est sensé avoir marché");
                            _context.SaveChanges();
                            return RedirectToPage("adminMenu");
                        }
                        else
                        {
                            Console.WriteLine("Wtf");
                            Console.WriteLine(result);
                            return RedirectToPage("user");
                        }

                    }
                    else
                    {
                        Console.WriteLine("SONT Pas Pareille");

                    }
                    //string hashedPassword = _passHasher.HashPassword(userBd, NewPassword);
                    //if (ConfPassword is not null)
                    //{
                    //    PasswordVerificationResult result = _passHasher.VerifyHashedPassword(userBd, hashedPassword, ConfPassword);
                    //    if(result is PasswordVerificationResult.Success)
                    //    {
                    //        user.PasswordHash = hashedPassword;
                    //        succeed = true;
                    //    }
                    //    else
                    //    {
                    //        return Page();
                    //    }
                    //}
                }
                //_context.SaveChanges();
                return RedirectToPage("adminMenu");
            }
            return RedirectToPage("index");
        }

    }
}
