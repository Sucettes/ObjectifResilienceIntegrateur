using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Newtonsoft.Json;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;

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
            public string intro { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string nomCategorie { get; set; }
            [Required]
            [DataType(DataType.Text)]
            public string descriptionCategorie { get; set; }
            public List<Categorie> lstCategories { get; set; }

            public List<RangeeTutoriel> lstRangeeTutoriels { get; set; }
            public List<Domain.Entities.Tutoriel> lstTutoriels { get; set; }
            public IFormFile imageRangeeFile { get; set; }
            public IFormFile imageBanierFile { get; set; }
            public string imgBannierUrl { get; set; }
        }

        public IActionResult OnGet()
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

        public IActionResult OnPostAsync()
        {
            UpdateInputData();
            return Page();
        }

        public IActionResult OnPostRedirectHomeTuto() => RedirectToPage("Index");


        public IActionResult OnPostCreeTutorielDetails(string intro, string id, string handler)
        {
            try
            {
                string imgUrl = null;
                string creationTutoStatus = "false";
                Input.id = id;
                Input.handler = handler;
                if (!(_db.Categories.Where(c => c.Nom == Input.cat).Count() == 0))
                {
                    Categorie cat = _db.Categories.Where(c => c.Nom == Input.cat).First();

                    if (cat != null && _db.Tutoriels.Where(t => t.Titre == Input.titre).Count() == 0)
                    {
                        Console.WriteLine(Input.imageBanierFile);
                        if (Input.imageBanierFile != null)
                        {
                            using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
                            {
                                using (var newMemoryStream = new MemoryStream())
                                {
                                    Input.imageBanierFile.CopyTo(newMemoryStream);
                                    var uploadRequest = new TransferUtilityUploadRequest
                                    {
                                        InputStream = newMemoryStream,
                                        Key = (DateTime.Now.Ticks + Input.imageBanierFile.FileName).ToString(), // filename
                                        BucketName = "mediafileobjectifresiliance", // bucket name of S3
                                        CannedACL = S3CannedACL.PublicRead,
                                    };

                                    var fileTransferUtility = new TransferUtility(client);
                                    fileTransferUtility.Upload(uploadRequest);
                                    imgUrl = ("https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/" + uploadRequest.Key).ToString();
                                }
                            }
                        }
                        Input.imgBannierUrl = imgUrl;
                        Domain.Entities.Tutoriel tuto = new Domain.Entities.Tutoriel();
                        tuto.Titre = Input.titre;
                        tuto.Duree = Input.duree;
                        tuto.Cout = Input.cout;
                        tuto.Difficulte = Input.difficulte;
                        tuto.Categorie = cat;
                        tuto.Introduction = intro;
                        tuto.LienImgBanniere = imgUrl;

                        if (tuto.EstValide())
                        {
                            _db.Tutoriels.Add(tuto);
                            _db.SaveChanges();

                            Input.id = _db.Tutoriels.Where(t => t.Titre == tuto.Titre).First().Id.ToString();
                            creationTutoStatus = "true";
                        }
                    }
                }
                UpdateInputData();
                return Redirect("/tutoriel/CreationTuto?handler=CreeTutorielDetails&id=" + Input.id + "&creationTutoStatus=" + creationTutoStatus);
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostModifieTutorielDetails(string intro, string id)
        {
            try
            {
                string modificationTutoStatus = "false";
                Input.id = id;
                Input.handler = "CreeTutorielDetails";
                if (!(_db.Categories.Where(c => c.Nom == Input.cat).Count() == 0))
                {
                    Categorie cat = _db.Categories.Where(c => c.Nom == Input.cat).First();
                    Domain.Entities.Tutoriel tuto = _db.Tutoriels.Where(t => t.Id == Guid.Parse(id)).First();
                    if (cat != null && tuto != null)
                    {
                        tuto.Titre = Input.titre;
                        tuto.Duree = Input.duree;
                        tuto.Cout = Input.cout;
                        tuto.Difficulte = Input.difficulte;
                        tuto.Categorie = cat;
                        tuto.Introduction = intro;


                        if (tuto.EstValide())
                        {
                            Input.id = tuto.Id.ToString();

                            _db.Tutoriels.Update(tuto);
                            _db.SaveChanges();

                            modificationTutoStatus = "true";
                        }
                    }
                }
                UpdateInputData();
                return Redirect("/tutoriel/CreationTuto?handler=CreeTutorielDetails&id=" + Input.id + "&modificationTutoStatus=" + modificationTutoStatus);
            }
            catch (Exception)
            {
                UpdateInputData();
                return Page();
            }
        }

        public IActionResult OnPostNettoyerTutorielDetails() => Redirect("/tutoriel/CreationTuto");

        public IActionResult OnPostCreationCategorie(string id, string handler)
        {
            try
            {
                string creationCategorieStatus = "false";
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
                        creationCategorieStatus = "true";
                    }
                }
                Input.id = id;
                Input.handler = handler;
                UpdateInputData();
                return Redirect("/Tutoriel/CreationTuto?handler=CreationCategorie&id=" + Input.id + "&creationCategorieStatus=" + creationCategorieStatus);

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
                string imgUrl = null;
                // TODO : Faire en sorte de validé si c'est une rangé d'image ou de texte ou les deux........
                if (positionImage == "right" || positionImage == "left")
                {
                    if (Input.imageRangeeFile != null)
                    {
                        using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
                        {
                            using (var newMemoryStream = new MemoryStream())
                            {
                                Input.imageRangeeFile.CopyTo(newMemoryStream);
                                var uploadRequest = new TransferUtilityUploadRequest
                                {
                                    InputStream = newMemoryStream,
                                    Key = (DateTime.Now.Ticks + Input.imageRangeeFile.FileName).ToString(), // filename
                                    BucketName = "mediafileobjectifresiliance", // bucket name of S3
                                    CannedACL = S3CannedACL.PublicRead,
                                };

                                var fileTransferUtility = new TransferUtility(client);
                                fileTransferUtility.Upload(uploadRequest);
                                imgUrl = ("https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/" + uploadRequest.Key).ToString();
                            }
                        }
                    }

                    RangeeTutoriel rangee = new RangeeTutoriel();
                    rangee.TutorielId = Guid.Parse(id);
                    rangee.Texte = rangeeTexte;
                    rangee.PositionImg = positionImage;
                    rangee.LienImg = imgUrl;

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
                return Redirect("/Tutoriel/Consultation?id=" + id + "&estPublie=true");
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

            if (!String.IsNullOrEmpty(Input.id))
            {
                Domain.Entities.Tutoriel tuto = _db.Tutoriels.Where(t => t.Id == Guid.Parse(Input.id)).First();
                if (tuto != null)
                {
                    Input.titre = tuto.Titre;
                    Input.duree = tuto.Duree;
                    Input.cout = tuto.Cout;
                    Input.difficulte = tuto.Difficulte;
                    Input.intro = tuto.Introduction;
                    Input.cat = tuto.Categorie.Nom;
                    Input.imgBannierUrl = tuto.LienImgBanniere;
                }
            }
        }
    }
}