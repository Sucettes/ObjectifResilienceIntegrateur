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
    public class TutorielIndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _db;

        public class InputModel
        {
            public List<Domain.Entities.Tutoriel> lstTutoriels { get; set; }

        }
        public TutorielIndexModel(GwenaelDbContext pDb) => _db = pDb;

        public async Task<IActionResult> OnGetAsync()
        {
            Input = new InputModel();
            Input.lstTutoriels = _db.Tutoriels.ToList<Domain.Entities.Tutoriel>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() => Page();

        public IActionResult OnPostRedirectCreationTuto()
        {
            return RedirectToPage("CreationTuto");
        }
    }
}