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
using Microsoft.AspNetCore.Components;


namespace Gwenael.Web.Pages
{
    public class CreationTutoModel : PageModel
    {
        private readonly GwenaelDbContext _db;

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public CreationTutoModel(GwenaelDbContext pDb) => _db = pDb;

        public class InputModel
        {
            [DataType(DataType.Text)]
            public string id { get; set; }
            public string handler { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string titre { get; set; }
            [Required]
            [DataType(DataType.Text)]
            public int difficulte { get; set; }
            [Required]
            [DataType(DataType.Currency)]
            public double cout { get; set; }
            [Required]
            [DataType(DataType.Text)]
            public int duree { get; set; }
            //[BindProperty(SupportsGet = true)]
            [DataType(DataType.Text)]
            public string cat { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string nomCategorie { get; set; }
            [Required]
            [DataType(DataType.Text)]
            public string descriptionCategorie { get; set; }
            public List<Categorie> lstCategories { get; set; }

            public List<RangeeTutoriel> lstRangeeTutoriels { get; set; }
            public List<Domain.Entities.Tutoriel> lstTutoriels { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Input = new InputModel();

            if (Request.Query.Count == 1)
            {
                Input.handler = Request.Query["handler"];
            }
            else if (Request.Query.Count > 1)
            {
                Input.handler = Request.Query["handler"];
                Input.id = Request.Query["id"];
            }
            UpdateInputData();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UpdateInputData();
            return Page();
        }

        public IActionResult OnPostRedirectHomeTuto()
        {
            return RedirectToPage("Index");
        }

        public IActionResult OnPostCreeTutorielDetails(string intro, string id, string handler)
        {
            try
            {
                Input.id = id;
                Input.handler = handler;
                if (!(_db.Categories.Where(c => c.Nom == Input.cat).Count() == 0))
                {
                    Categorie cat = _db.Categories.Where(c => c.Nom == Input.cat).First();

                    if (cat != null && _db.Tutoriels.Where(t => t.Titre == Input.titre).Count() == 0)
                    {
                        Domain.Entities.Tutoriel tuto = new Domain.Entities.Tutoriel();
                        tuto.Titre = Input.titre;
                        tuto.Duree = Input.duree;
                        tuto.Cout = Input.cout;
                        tuto.Difficulte = Input.difficulte;
                        tuto.Categorie = cat;
                        tuto.Introduction = intro;

                        if (tuto.EstValide())
                        {
                            _db.Tutoriels.Add(tuto);
                            _db.SaveChanges();

                            Input.id = _db.Tutoriels.Where(t => t.Titre == tuto.Titre).First().Id.ToString();
                        }
                    }
                }
                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostCreationCategorie(string id, string handler)
        {
            try
            {
                if (_db.Categories.Where(c => c.Nom == Input.nomCategorie).Count() == 0)
                {
                    Categorie cat = new Categorie();
                    cat.Nom = Input.nomCategorie;
                    cat.Description = Input.descriptionCategorie;

                    if (cat.EstValide())
                    {
                        _db.Categories.Add(cat);
                        _db.SaveChanges();

                        Input.descriptionCategorie = "";
                        Input.nomCategorie = "";
                    }
                }
                Input.id = id;
                Input.handler = handler;
                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostAjoutRangee(string rangeeTexte, string positionImage, string id, string handler)
        {
            try
            {
                Input.id = id;
                Input.handler = handler;

                // TODO : Faire en sorte de validé si c'est une rangé d'image ou de texte ou les deux........
                if (positionImage == "right" || positionImage == "left")
                {
                    RangeeTutoriel rangee = new RangeeTutoriel();
                    rangee.TutorielId = Guid.Parse(id);
                    rangee.Texte = rangeeTexte;
                    rangee.PositionImg = positionImage;
                    // Mettre l'url de l'image

                    _db.RangeeTutoriels.Add(rangee);
                    _db.SaveChanges();
                    Guid rId = _db.RangeeTutoriels.Where(r => r == rangee).First().Id;

                    Input.lstRangeeTutoriels = _db.RangeeTutoriels.Where(r => r.TutorielId == Guid.Parse(id)).ToList<RangeeTutoriel>();
                }

                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {

                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostPublierTutoriel(string id, string handler)
        {
            try
            {
                Input.id = id;
                Input.handler = handler;

                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }


        public IActionResult OnPostTutoChanger(string tutoId, string handler)
        {
            try
            {
                Input.handler = "AjoutRangee";
                Input.id = tutoId;
                Input.handler = handler;
                UpdateInputData();
                return Redirect("/tutoriel/CreationTuto?handler=TutoChanger&id=" + tutoId);
            }
            catch (Exception)
            {
                UpdateInputData();
                return Redirect("/tutoriel/CreationTuto?handler=TutoChanger");
            }
        }

        public IActionResult OnPostEditRange(string idRangeeVal, string id, string handler)
        {
            try
            {
                Input.handler = "AjoutRangee";
                Input.id = id;

                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public class Rangee
        {
            public string idRangeeVal { get; set; }
        }

        public IActionResult OnPostDeleteRange(string id, string handler, [FromBody] Rangee rangee)
        {
            try
            {
                Input.handler = "AjoutRangee";
                Input.id = id;

                Domain.Entities.Tutoriel t = _db.Tutoriels.Where(t => t.Id == Guid.Parse(id) && t.EstPublier == false).First();
                if (t != null)
                {
                    RangeeTutoriel rt = _db.RangeeTutoriels.Where(r => r.TutorielId == Guid.Parse(id) && r.Id == Guid.Parse(rangee.idRangeeVal)).First();
                    if (rt != null)
                    {

                        _db.RangeeTutoriels.Remove(rt);
                        _db.SaveChanges();
                    }
                }

                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostPublieTuto(string id, string handler)
        {
            try
            {
                Input.handler = "AjoutRangee";
                Input.id = id;

                Domain.Entities.Tutoriel t = _db.Tutoriels.Where(t => t.Id == Guid.Parse(id) && t.EstPublier == false).First();
                if (t != null)
                {
                    t.EstPublier = true;
                    _db.Tutoriels.Update(t);
                    _db.SaveChanges();
                }

                UpdateInputData();
                return Page();
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public void UpdateInputData()
        {
            Input.lstCategories = _db.Categories.ToList<Categorie>();
            if (!String.IsNullOrEmpty(Input.id))
            {
                Input.lstRangeeTutoriels = _db.RangeeTutoriels.Where(r => r.TutorielId == Guid.Parse(Input.id)).ToList<RangeeTutoriel>();
            }
            Input.lstTutoriels = _db.Tutoriels.Where(t => t.EstPublier == false).ToList<Domain.Entities.Tutoriel>();
        }
    }
}