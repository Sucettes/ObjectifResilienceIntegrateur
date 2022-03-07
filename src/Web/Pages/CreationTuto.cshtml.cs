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
    public class CreationTutoModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        private readonly GwenaelDbContext _context;

        public CreationTutoModel(GwenaelDbContext context)
        {
            _context = context;
        }

        public class InputModel
        {
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}