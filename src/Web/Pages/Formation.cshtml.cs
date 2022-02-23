using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;

namespace Gwenael.Web.Pages
{
    public class FormationModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public FormationModel(GwenaelDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public Formation Formation { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id.ToString() is null or "")
            {
                return Page();
            }
            else
            {
                Formation = await _context.Formations.FindAsync(id);
                ViewData["formation"] = Formation;
                return Page();
            }

        }
        public async Task<IActionResult> OnPost(string btnSave, string btnDelete, string btnAdd)
        {
            Formation formationBD = null;
            try
            {
                formationBD = await _context.Formations.FindAsync(Formation.Id);
            }
            catch
            {
            }
            //if (ModelState.IsValid)
            //{
            //}
            if (btnSave != null)
            {
                formationBD.Name = Formation.Name;
                formationBD.Description = Formation.Description;

                await _context.SaveChangesAsync();
                return RedirectToPage("adminMenu");
            }
            else if (btnDelete != null)
            {
                _context.Formations.Remove(formationBD);
                await _context.SaveChangesAsync();
                return RedirectToPage("adminMenu");
            }
            else if (btnAdd != null)
            {
                //Ajout Manuel d'un user
                Formation newFormation = new Formation
                {
                    Name = Formation.Name,
                    Description = Formation.Description
                };
                _context.Add<Formation>(newFormation);
                _context.SaveChanges();
                return RedirectToPage("adminMenu");
            }
            return RedirectToPage("index");



        }

    }
}
