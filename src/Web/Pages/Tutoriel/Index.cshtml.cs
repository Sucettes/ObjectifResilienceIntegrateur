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

        }
        public TutorielIndexModel(GwenaelDbContext pDb) => _db = pDb;

        //public Guid ObtenirIdDuUserSelonEmail(string email)
        //{
        //    User user = (User)_db.Users.Where(u => u.UserName == email).First();
        //    return user.Id;
        //}
        public IActionResult OnGet()
        {
            Input = new InputModel();
            //Input.droitAccess = false;
            Input.droitAccess = true; // a supprimer
            //if (User.Identity.IsAuthenticated)
            //{
            //    Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
            //    if (Permission.VerifierAccesGdC(idConnectedUser, _db))
            //    {
            //        Input.droitAccess = true;
            //    }
            //}
            Input.lstTutoriels = _db.Tutos.ToList();

            return Page();
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectCreationTuto() => RedirectToPage("CreationTuto");

        public class RechercherFiltre
        {
            public bool radioFiltreEstPublie { get; set; }
            public string radioFiltre { get; set; }
            public string rechercheValeur { get; set; }
        }
        public IActionResult OnPostRecherche([FromForm] RechercherFiltre formData)
        {
            try
            {
                List<Tutos> t = new();
                if (formData.radioFiltre == "radioFiltreTitre")
                {
                    if (!formData.rechercheValeur.IsNullOrEmpty())
                    {
                        t = _db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie && t.Titre.Contains(formData.rechercheValeur)).ToList();
                    }
                    else
                    {
                        t = _db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie).ToList();
                    }
                }
                else if (formData.radioFiltre == "radioFiltreCategorie")
                {
                    if (!formData.rechercheValeur.IsNullOrEmpty())
                    {
                        List<CategoriesTutos> ct = _db.CategoriesTutos.Where(c => c.Nom.Contains(formData.rechercheValeur)).ToList();
                        foreach (var i in ct)
                        {
                            List<Tutos> iTemp = _db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie && t.Categorie == i).ToList();

                            foreach (var jTemp in iTemp)
                            {
                                t.Add(jTemp);
                            }
                        }
                    }
                    else
                    {
                        t = _db.Tutos.Where(t => t.EstPublier == formData.radioFiltreEstPublie).ToList();
                    }

                }
                return t != null ? StatusCode(201, new JsonResult(t)) : StatusCode(400);
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }
    }
}