using Gwenael.Domain;
using Gwenael.Web;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Controllers
{
    public class TutorielController : Controller
    {
        GwenaelDbContext context;
        public TutorielController(GwenaelDbContext pcontext)
        {
            context = pcontext;
        }

        // GET: TutorielController
        public ActionResult Index(GwenaelDbContext pcontext)
        {
            context = pcontext;
            return View();
        }

        // GET: TutorielController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TutorielController/CreationTuto
        public ActionResult CreationTuto()
        {
            return View();
        }

        // POST: TutorielController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationTuto(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TutorielController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TutorielController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TutorielController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TutorielController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }





        // POST: TutorielController/CreationCategorie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreationCategorie(IFormCollection collection)
        {
            try
            {
                string nom = "";
                string desc = "";
                if(collection.TryGetValue("formDesc", out var descOut))
                {
                    desc = descOut;
                }                
                if(collection.TryGetValue("formNom", out var nomOut))
                {
                    nom = nomOut;
                }

                if(nom != "" && desc != "")
                {
                    Categorie newCategorie = new Categorie
                    {
                        Nom = nom,
                        Description = desc
                    };
                    context.Categories.Add(newCategorie);
                    context.SaveChanges();
                }

                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                //return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
