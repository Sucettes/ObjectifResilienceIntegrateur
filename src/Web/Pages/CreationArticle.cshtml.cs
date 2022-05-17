using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Web.FctUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages
{
    public class CreationArticle : PageModel
    {
        private readonly GwenaelDbContext _context;
        private IList<NewPage> NewPages { get; set; }


        public CreationArticle(GwenaelDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Article article { get; set; }

        [BindProperty(SupportsGet = true)]
        public Media media { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<Article> Articles { get; set; }

        public string NewArticleCreated;

        public IActionResult  OnGet()
        {
            NewPages = _context.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            Input = new InputModel();
            string idArticle;

            if (Request.Query.Count > 0 && Request.Query.ContainsKey("id"))
            {
                
                Articles = _context.Articles.ToList();
                idArticle = Request.Query["id"];
                int intId = Int32.Parse(idArticle);
                Article b = Articles.Where(article => article.Id == intId).First();
                Input.TextArea = b.InerText;
                Input.Titre = b.Titre;
                ViewData["Modifier"] = "true";
            }
            NewArticleCreated = "false";
            ViewData["NewArticleCreated"] = NewArticleCreated;
            return Page();
        }

        public class InputModel
        {
            public IFormFile[] FormFile { get; set; }

            public string TextArea { get; set; }
            public string Titre { get; set; }

        }


        public async Task<IActionResult> OnPost(string titre, string inerText)
        {

            Article newArticle = new Article
            {
                Titre = titre,
                InerText = inerText,
                EstPublier = false

            };

            NewPages = _context.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            if (Request.Query.Count > 0 && Request.Query.ContainsKey("id"))
            {
                string idArticle = Request.Query["id"];
                int intId = Int32.Parse(idArticle);
                Article b = _context.Articles.Where(b => b.Id == intId).First();
                b.Titre = titre;
                b.InerText = inerText;
                _context.Articles.Update(b);
            }

            if (Input.FormFile != null)
            {
                var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1);
                var File = Input.FormFile[0];
                using (var newMemoryStream = new MemoryStream())
                {

                    File.CopyTo(newMemoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = File.FileName, // filename
                        BucketName = "mediafileobjectifresiliance", // bucket name of S3
                        CannedACL = S3CannedACL.PublicRead,
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                string link =  File.FileName;
                newArticle.LienImg = link;

            }

            if (Request.Query.Count == 0 && !Request.Query.ContainsKey("id"))
            {
               _context.Articles.Add(newArticle);
            }

            NewArticleCreated = "true";
            ViewData["NewArticleCreated"] = NewArticleCreated;

            _context.SaveChanges();
            
            return Page();
        }

    }
}
