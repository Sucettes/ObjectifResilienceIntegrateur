using Gwenael.Application.Mailing;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly GwenaelDbContext _context;
        public IList<NewPage> NewPages { get; set; }
        private readonly GwenaelDbContext _db;

        public RegisterModel(GwenaelDbContext context, UserManager<User> userManager,
            GwenaelDbContext pDb)
        {
            _context = context;
            _userManager = userManager;
            _db = pDb;

        }
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl = null)
        {
            NewPages = _db.NewPages.ToList();
            ViewData["NewPages"] = NewPages;
            ReturnUrl = returnUrl;
        }
        public class RegisterForm
        {
            public string courriel { get; set; }
            public string password { get; set; }
            public string confPassword { get; set; }
        }
        public async Task<IActionResult> OnPostAsync([FromForm] RegisterForm registerForm)
        {

            NewPages = await _db.NewPages.ToListAsync();
            ViewData["NewPages"] = NewPages;

            Dictionary<string, string> dictError = new Dictionary<string, string>();
            if (registerForm.password.Length < 6 || registerForm.password.Length > 100 || registerForm.confPassword.Length > 100 || registerForm.confPassword.Length < 6 && registerForm.confPassword != registerForm.password)
            {
                dictError.Add("erreurPassword", "Le mot de passe doit contenir un minimum de 6 caractères et un maximum de 100 caractères.");
            }
            else
            {
                if (registerForm.confPassword != registerForm.password && dictError.Count != 3)
                    dictError.Add("erreurPassword", "Les mots de passe entrées ne correspondent pas.");
            }

            if (!Regex.IsMatch(registerForm.courriel, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                dictError.Add("erreurCourriel", "Le courriel n'est pas valide.");
            }
            else
            {
                User userBd = null;
                bool erreur = false;
                try
                {
                    userBd = _context.Users.Where(u => u.UserName == registerForm.courriel).First();
                    Console.WriteLine(userBd);
                    erreur = true;
                }
                catch
                {
                }
                if (erreur)
                {
                    dictError.Add("erreurCourriel", "Le courriel n'est pas disponible.");
                }
            }
            if (dictError.Count > 0)
            {
                return new JsonResult(dictError);
            }
            else
            {
                User user = new User { UserName = registerForm.courriel, Email = registerForm.courriel, FirstName = "", LastName = "" };
                var result = await _userManager.CreateAsync(user, registerForm.password);
                if (result.Succeeded)
                {
                    dictError.Add("msgSuccess", "Nous avons bien reçu votre demande. Elle sera traitée dans les plus brefs délais. Vous allez être redirigé dans 10 secondes.");
                    return new JsonResult(dictError);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return new JsonResult(Response);
            }


        }
    }
}
