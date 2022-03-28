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


namespace Gwenael.Web.Pages
{
    public class ConsultationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _db;

        public class InputModel
        {
            public Domain.Entities.Tutoriel tutoriel { get; set; }
            public List<RangeeTutoriel> lstRangeeTuto { get; set; }
            public string id { get; set; }

        }
        public ConsultationModel(GwenaelDbContext pDb) => _db = pDb;

        public IActionResult OnGet()
        {
            bool estValide = false;
            Input = new InputModel();

            if (Request.Query.Count >= 1)
            {
                Input.id = Request.Query["id"];
                if (IdEstValide()) estValide = true;
            }

            if (estValide) return Page();
            else return Redirect("/tutoriel");
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectHomeTuto() => RedirectToPage("Index");

        public IActionResult OnPostDeleteTuto(string handler, [FromBody] TutorielIdVal tutoVal)
        {
            try
            {
                Domain.Entities.Tutoriel tuto = _db.Tutoriels.Where(t => t.Id == Guid.Parse(tutoVal.tutorielIdVal)).First();
                _db.Tutoriels.Remove(tuto);
                _db.SaveChanges();

                return Redirect("/tutoriel?deleteStatus=true");
            }
            catch (Exception)
            {
                return Redirect("/tutoriel");
            }
        } 

        public IActionResult OnPostUnpublishTuto(string handler, [FromBody] TutorielIdVal tutoVal)
        {
            try
            {
                Domain.Entities.Tutoriel tuto = _db.Tutoriels.Where(t => t.Id == Guid.Parse(tutoVal.tutorielIdVal)).First();
                tuto.EstPublier = false;
                _db.SaveChanges();

                return Redirect("/tutoriel?unPublishStatus=true");
            }
            catch (Exception)
            {
                return Redirect("/tutoriel");
            }
        }

        public class TutorielIdVal
        {
            public string tutorielIdVal { get; set; }
        }

        private bool IdEstValide()
        {
            bool estValide = false;

            if (!Input.id.IsNullOrEmpty())
            {
                Input.tutoriel = _db.Tutoriels.Where(t => t.Id == Guid.Parse(Input.id) && t.EstPublier == true).First();
                if (Input.tutoriel != null)
                {
                    GetContenue();
                    estValide = true;
                }
            }

            return estValide;
        }

        private void GetContenue() => Input.lstRangeeTuto = _db.RangeeTutoriels.Where(r => r.TutorielId == Guid.Parse(Input.id)).ToList();
    }
}