using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using IdentityServer4.Extensions;
using Gwenael.Web.FctUtils;

namespace Gwenael.Web.Pages
{
    public class ArticleIndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }
        private IList<NewPage> NewPages { get; set; }


        private readonly GwenaelDbContext _db;

        public class InputModel
        {
            public List<Domain.Entities.Article> lstArticles { get; set; }
            public bool droitAccess { get; set; }

        }

        public class RechercherFiltre
        {
            public bool radioFiltreEstPublie { get; set; }
            public string radioFiltre { get; set; }

            public string rechercheValeur { get; set; }
        }

        public ArticleIndexModel(GwenaelDbContext pDb) => _db = pDb;

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                if (Permission.EstAdministrateur(idConnectedUser, _db))
                {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }


            NewPages = _db.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

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

            Input.lstArticles = _db.Articles.ToList();

            return Page();
        }

        public IActionResult OnPost() => Page();

        public IActionResult OnPostRedirectCreationArticle()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                if (Permission.EstAdministrateur(idConnectedUser, _db))
                {
                    ViewData["estAdmin"] = "true";
                }
            }
            else
            {
                ViewData["estAdmin"] = "false";
            }
            return RedirectToPage("CreationArticle");
        }

        public IActionResult OnPostRecherche([FromForm] RechercherFiltre formData)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                    if (Permission.EstAdministrateur(idConnectedUser, _db))
                    {
                        ViewData["estAdmin"] = "true";
                    }
                }
                else
                {
                    ViewData["estAdmin"] = "false";
                }
                List<Article> lstArticles = new List<Article>();
                //if (!formData.cat.IsNullOrEmpty() && formData.cat != "Toutes")
                //{
                //    List<CategoriesTutos> cats = _db.CategoriesTutos.Where(c => c.Nom.Contains(formData.cat)).ToList();
                //    if (cats.Count > 0)
                //    {
                //        if (!formData.rechercheValeur.IsNullOrEmpty())
                //        {
                //            t = _db.Tutos.Where(t => t.EstPublier ==
                //            formData.radioFiltreEstPublie && t.Categorie == cats[0]
                //            && t.Titre.Contains(formData.rechercheValeur)).ToList();
                //        }
                //        else
                //        {
                //            t = _db.Tutos.Where(t => t.EstPublier ==
                //            formData.radioFiltreEstPublie && t.Categorie == cats[0]).ToList();
                //        }
                //    }
                //}
                if (!formData.rechercheValeur.IsNullOrEmpty())
                {
                    lstArticles.AddRange(_db.Articles.Where(article => article.EstPublier == formData.radioFiltreEstPublie
                     && article.Titre.Contains(formData.rechercheValeur)).ToList());
                }
                //if (!formData.rechercheValeur.IsNullOrEmpty())
                //{
                //    lstArticles.AddRange(_db.Articles.Where(article => article.Titre.Contains(formData.rechercheValeur)).ToList());
                //}
                else
                {
                    lstArticles.AddRange(_db.Articles.Where(article => article.EstPublier == formData.radioFiltreEstPublie ).ToList());             
                }
                return StatusCode(201, new JsonResult(lstArticles));
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }


        public Guid ObtenirIdDuUserSelonEmail(string email)
        {
            User user = (User)_db.Users.Where(u => u.UserName == email).First();
            return user.Id;
        }

        public IActionResult OnPostObtenirDroit()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Guid idConnectedUser = FctUtils.Permission.ObtenirIdDuUserSelonEmail(User.Identity.Name, _db);
                    if (Permission.EstAdministrateur(idConnectedUser, _db))
                    {
                        ViewData["estAdmin"] = "true";
                    }
                }
                else
                {
                    ViewData["estAdmin"] = "false";
                }

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
