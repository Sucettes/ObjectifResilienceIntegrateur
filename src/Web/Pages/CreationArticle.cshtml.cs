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

        public void OnGet()
        {
            Input = new InputModel();
        }

        public class InputModel
        {
            public IFormFile[] FormFile { get; set; }

        }


        public async Task<IActionResult> OnPost(string titre, string inerText)
        {

            // Upload de l'article et sont titre 
            int idNewArticle = 0;
            Article newArticle = new Article
            {
                Titre = titre,
                InerText = inerText
            };

            //await _context.SaveChangesAsync();
            idNewArticle = newArticle.Id;


            Media newMedia = new Media();
            var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1);
            // mettre dans app setting
            //using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
            //{

            //foreach (IFormFile File in Input.FormFile)
            //{
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

            string link = "s3/mediafileobjectifresiliance/" + File.FileName;


            newArticle.LienImg = link;
            _context.Articles.Add(newArticle);
            //newMedia.Id = File.FileName;
            //newMedia.Idreference = idNewArticle;
            //newMedia.LinkS3 = link;
            //newMedia.OrderInThePage = 0;


            //_context.Medias.Add(newMedia);
            //Console.WriteLine(newMedia.ToString());
            //}
            //}

            _context.SaveChanges();
            return Page();
        }

    }
}
