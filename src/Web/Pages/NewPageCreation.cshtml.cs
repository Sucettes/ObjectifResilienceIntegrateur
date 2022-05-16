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
    public class NewPageCreation : PageModel
    {
        private readonly GwenaelDbContext _context;

        public NewPageCreation(GwenaelDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Article article { get; set; }

        [BindProperty(SupportsGet = true)]
        public Media media { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<NewPage> NewPages { get; set; }

        public IActionResult OnGet()
        {
            Input = new InputModel();
            string idNewPage;


            NewPages = _context.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            if (Request.Query.Count > 0 && Request.Query.ContainsKey("id"))
            {



                idNewPage = Request.Query["id"];
                int intId = Int32.Parse(idNewPage);
                NewPage b = NewPages.Where(newPage => newPage.Id == intId).First();
                Input.TextArea = b.InerText;
                Input.Titre = b.Titre;
                ViewData["Modifier"] = "true";
            }

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

            NewPages = _context.NewPages.ToList();
            ViewData["NewPages"] = NewPages;

            NewPage newPage = new NewPage
            {
                Titre = titre,
                InerText = inerText,
                EstPublier = false

            };
            if (Request.Query.Count > 0 && Request.Query.ContainsKey("id"))
            {
                string idNewPage = Request.Query["id"];
                int intId = Int32.Parse(idNewPage);
                NewPage b = _context.NewPages.Where(b => b.Id == intId).First();
                b.Titre = titre;
                b.InerText = inerText;
                _context.NewPages.Update(b);
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

                string link = File.FileName;
                newPage.LienImg = link;

            }

            if (Request.Query.Count == 0 && !Request.Query.ContainsKey("id"))
            {
                _context.NewPages.Add(newPage);
            }

            _context.SaveChanges();

            return Page();
        }

    }
}
