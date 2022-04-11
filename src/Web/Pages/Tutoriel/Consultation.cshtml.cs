using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
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
            public Tutos tutoriel { get; set; }
            public List<RangeeTutos> lstRangeeTuto { get; set; }
            public string id { get; set; }
        }
        public ConsultationModel(GwenaelDbContext pDb) => _db = pDb;

        public IActionResult OnGet()
        {
            Input = new InputModel();

            if (Request.Query.Count >= 1)
            {
                Input.id = Request.Query["id"];
                if (IdEstValide()) return Page();
            }

            return Redirect("/tutoriel");
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectHomeTuto() => RedirectToPage("Index");

        public IActionResult OnPostDeleteTuto([FromBody] TutorielIdVal tutoVal)
        {
            try
            {
                _db.Tutos.Remove(entity: _db.Tutos.Where(t => t.Id == Guid.Parse(tutoVal.tutorielIdVal)).First());
                _db.SaveChanges();

                return Redirect("/tutoriel?deleteStatus=true");
            }
            catch (Exception)
            {
                return Redirect("/tutoriel");
            }
        }

        public IActionResult OnPostUnpublishTuto([FromBody] TutorielIdVal tutoVal)
        {
            try
            {
                _db.Tutos.Where(t => t.Id == Guid.Parse(tutoVal.tutorielIdVal)).First().EstPublier = false;
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
            if (!Input.id.IsNullOrEmpty())
            {
                Input.tutoriel = _db.Tutos.Where(t => t.Id == Guid.Parse(Input.id) && t.EstPublier == true).First();
                if (Input.tutoriel != null)
                {
                    GetContenue();
                    return true;
                }
            }
            return false;
        }

        private void GetContenue() => Input.lstRangeeTuto = _db.RangeeTutos.Where(r => r.TutorielId == Guid.Parse(Input.id)).ToList();
    }
}