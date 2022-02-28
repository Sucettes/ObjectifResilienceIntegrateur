using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol;
using OfficeOpenXml;
using Spk.Common.Helpers.String;


namespace Gwenael.Web.Pages
{
    public class InviteFormationModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _context;

        public InviteFormationModel(GwenaelDbContext context)
        {
            _context = context;
        }

        public class InputModel
        {
            [EmailAddress]
            public string Email { get; set; }
            public List<EmailFormation> lstEmailFormation { get; set; }
            public IFormFile FormFile { get; set; }

        }
        public void OnGet()
        {
            Input = new InputModel();
            Console.WriteLine("GET-----------------");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.FormFile != null && Input.FormFile.FileName.EndsWith(".csv"))
            {
                List<EmailFormation> list = new List<EmailFormation>();

                using (var reader = new System.IO.StreamReader(Input.FormFile.OpenReadStream()))
                {
                    string result = reader.ReadToEnd();
                    result = result.Replace("\r", "");
                    string[] vectLignes = result.Split('\n');
                    int nbLignes = vectLignes.Length - 2;

                    if (vectLignes[vectLignes.Length - 1] != "")
                    {
                        nbLignes = vectLignes.Length - 1;
                    }

                    string formation = "FormationPlaceHolder";

                    for (int i = 0; i <= nbLignes; i++)
                    {
                        string[] vectChamps = vectLignes[i].Split(';');
                        if(vectChamps[0] != "" && vectChamps[0].Length > 1)
                            list.Add(new EmailFormation(formation, vectChamps[0]));
                    }

                }
                Input.lstEmailFormation = list;
                CreeUserEmail();
            }

            return Page();
        }

        private async void CreeUserEmail()
        {
            try
            {
                foreach (var item in Input.lstEmailFormation)
                {
                    User newUser = new User
                    {
                        Email = item.GetEmail(),
                        UserName = item.GetEmail().Split('@')[0]
                    };

                    //int index = Input.lstEmailFormation.IndexOf(item);

                    _context.Add<User>(newUser);
                    _context.SaveChanges();

                    //if (_context.Users.Contains(newUser))
                    //{
                    //    Input.lstEmailFormation[index].SetStatus("Erreur");
                    //}
                    //else
                    //{
                    //    _context.Add<User>(newUser);
                    //    Input.lstEmailFormation[index].SetStatus("Réussie");
                    //    _context.SaveChanges();
                    //}
                }
            }
            catch
            {
            }
        }

        public class EmailFormation
        {
            private string _formation;
            private string _email;
            private string _status;

            public EmailFormation(string formation, string email)
            {
                _formation = formation;
                _email = email;
                _status = "";
            }

            public EmailFormation(string formation, string email, string status)
            {
                _formation = formation;
                _email = email;
                _status = _status;
            }

            public string GetEmail()
            {
                return _email;
            }

            public string GetFormation()
            {
                return _formation;
            }

            public string GetStatus()
            {
                return _status;
            }

            public string SetStatus(string status)
            {
                _status = status;
                return _status;
            }
        }
    }
}
