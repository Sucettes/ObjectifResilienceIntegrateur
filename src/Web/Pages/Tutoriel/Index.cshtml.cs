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
using Spk.Common.Helpers.String;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class TutorielIndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _db;

        public class InputModel
        {
            public List<Tutos> lstTutoriels { get; set; }
            public bool droitAccess { get; set; }
            public List<CategoriesTutos> lstCategories { get; set; }

        }
        public TutorielIndexModel(GwenaelDbContext pDb) => _db = pDb;

        public Guid ObtenirIdDuUserSelonEmail(string email)
        {
            User user = (User)_db.Users.Where(u => u.UserName == email).First();
            return user.Id;
        }
        public IActionResult OnGet()
        {
            Input = new InputModel();
            Input.droitAccess = false;
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
                if (Permission.VerifierAccesGdC(idConnectedUser, _db))
                {
                    Input.droitAccess = true;
                }
            }
            Input.lstTutoriels = _db.Tutos.ToList();
            Input.lstCategories = _db.CategoriesTutos.ToList();

            return Page();
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectCreationTuto() => RedirectToPage("CreationTuto");

        public class RechercherFiltre
        {
            public bool radioFiltreEstPublie { get; set; }
            public string radioFiltre { get; set; }
            public string rechercheValeur { get; set; }
            public string cat { get; set; }
        }

        public IActionResult OnPostRecherche([FromForm] RechercherFiltre formData)
        {
            try
            {
                List<Tutos> t = new List<Tutos>();
                if (!formData.cat.IsNullOrEmpty() && formData.cat != "Toutes")
                {
                    List<CategoriesTutos> cats = _db.CategoriesTutos.Where(c => c.Nom.Contains(formData.cat)).ToList();
                    if (cats.Count > 0)
                    {
                        if (!formData.rechercheValeur.IsNullOrEmpty())
                        {
                            t = _db.Tutos.Where(t => t.EstPublier ==
                            formData.radioFiltreEstPublie && t.Categorie == cats[0]
                            && t.Titre.Contains(formData.rechercheValeur)).ToList();
                        }
                        else
                        {
                            t = _db.Tutos.Where(t => t.EstPublier ==
                            formData.radioFiltreEstPublie && t.Categorie == cats[0]).ToList();
                        }
                    }
                }
                else if (!formData.rechercheValeur.IsNullOrEmpty())
                {
                    List<CategoriesTutos> catTemp = _db.CategoriesTutos.ToList();
                    foreach (var item in catTemp)
                    {
                        t.AddRange(_db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie
                        && t.Categorie == item && t.Titre.Contains(formData.rechercheValeur)).ToList());
                    }
                }
                else
                {
                    List<CategoriesTutos> catTemp = _db.CategoriesTutos.ToList();
                    foreach (var item in catTemp)
                    {
                        t.AddRange(_db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie && t.Categorie == item).ToList());
                    }
                }
                return StatusCode(201, new JsonResult(t));
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }

        public IActionResult OnPostObtenirDroit()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
                    if (Permission.VerifierAccesGdC(idConnectedUser, _db))
                    {
                        return StatusCode(200, true);
                    }
                }
                return StatusCode(200, false);
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }
    }
}