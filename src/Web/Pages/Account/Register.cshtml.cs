using Gwenael.Application.Mailing;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailFactory _emailFactory;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            IEmailFactory emailFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailFactory = emailFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Le mot de passe doit contenir un minimum de 6 caractères et un maximum de 100 caractères.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Vos deux mots de passe entrés ne correspondent pas.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }
        public class RegisterForm
        {
            [Required]
            [EmailAddress]
            public string courriel { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "Le mot de passe doit contenir un minimum de 6 caractères et un maximum de 100 caractères.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string password { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Vos deux mots de passe entrés ne correspondent pas.")]
            public string confPassword { get; set; }
        }
        public async Task<IActionResult> OnPostAsync([FromForm] RegisterForm registerForm)
        {
            Dictionary<string, string> dictError = new Dictionary<string, string>();
            if (registerForm.password.Length < 6 || registerForm.password.Length > 100 || registerForm.confPassword.Length > 100 || registerForm.confPassword.Length < 6 && registerForm.confPassword != registerForm.password)
            {
                dictError.Add("erreurPassword", "Le mot de passe doit contenir un minimum de 6 caractères et un maximum de 100 caractères.");
            }
            else if (registerForm.confPassword != registerForm.password && dictError.Count != 3)
                dictError.Add("erreurPassword", "Les mots de passe entrées ne correspondent pas.");
            if (!Regex.IsMatch(registerForm.courriel, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                dictError.Add("erreurCourriel", "Le courriel n'est pas valide.");
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
                    Console.WriteLine("Yes");
                    return new JsonResult(dictError);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                Console.WriteLine("Oups");
                // If we got this far, something failed, redisplay form
                return new JsonResult(Response);
            }


        }
    }
}
