using Gwenael.Application.Mailing;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            IEmailFactory emailFactory)
        {
            _userManager = userManager;
        }
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl = null)
        {
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
            Dictionary<string, string> dictError = new Dictionary<string, string>();
            if (registerForm.password.Length < 6 || registerForm.password.Length > 100 || registerForm.confPassword.Length > 100 || registerForm.confPassword.Length < 6 && registerForm.confPassword != registerForm.password)
            {
                dictError.Add("erreurPassword", "Le mot de passe doit contenir un minimum de 6 caractères et un maximum de 100 caracteres.");
            }
            else
            {
                if (registerForm.confPassword != registerForm.password && dictError.Count != 3)
                    dictError.Add("erreurPassword", "Les mots de passe entrées ne correspondent pas.");
            }

            if (!Regex.IsMatch(registerForm.courriel, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                dictError.Add("erreurCourriel", "Le courriel nest pas valide.");
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
                    dictError.Add("msgSuccess", "Nous avons bien recu votre demande. Elle sera traitee dans les plus brefs delais. Vous allez etre redirige dans 10 secondes.");
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
