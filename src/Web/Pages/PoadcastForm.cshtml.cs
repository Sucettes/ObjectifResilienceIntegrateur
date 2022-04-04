using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


namespace Gwenael.Web.Pages
{
    public class PoadcastFormModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public PoadcastFormModel(GwenaelDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public Poadcast poadcast { get; set; }
        public IList<Poadcast> poadcasts { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        //public IHostingEnvironment env { get; set; }
        public void OnGet()
        {
            Input = new InputModel();
            
        }

        public class InputModel
        {
            public IFormFile[] FormFile { get; set; }

        }
        
        public async Task<IActionResult> OnPost(string titre, string description, string categorie)
        {
           

                if (description == null)
                {
                description = "aucune description disponible pour ce poadcast";
                }

            // mettre dans app setting
            using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
            {

                foreach (IFormFile File in Input.FormFile)
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        File.CopyTo(newMemoryStream);
                        Debug.WriteLine("My debug string here");
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = File.FileName, // filename
                            BucketName = "mediafileobjectifresiliance", // bucket name of S3
                            CannedACL = S3CannedACL.PublicRead,
                        };

                        var fileTransferUtil = new TransferUtility(client);
                        await fileTransferUtil.UploadAsync(uploadRequest);
                    }

                    Poadcast poadcast = new Poadcast
                    {

                        ID = File.FileName,
                        titre = titre,
                        url = "https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/" + File.FileName,
                        description = description,
                        categorie = categorie
                    };


                    _context.Poadcasts.Add(poadcast);
                    await _context.SaveChangesAsync();

                }
                return RedirectToPage("poadcastPage");
            }
        }
    }
    //[HttpPost]
        //public IActionResult Upload(IFormFile file, [FromServices] IHostingEnvironment oHostingEnvironment)
        //{
        //    string fileName = $"{oHostingEnvironment.WebRootPath}\\images\\{file.FileName}";

        //    using (FileStream fileStream = System.IO.File.Create(fileName))
        //    {
        //        file.CopyTo(fileStream);
        //        fileStream.Flush();
        //    }

        //    //ViewData["message"] = $"File uploaded Successful. File"
        //    return RedirectToPage("index");
        //}
        
    //foreach (IFormFile file in Input.FormFile)
            //{
            //    string fileName = $"{env.WebRootPath}\\images\\{file.FileName}";

            //    using (FileStream fileStream = System.IO.File.Create(fileName))
            //    {
            //        file.CopyTo(fileStream);
            //        fileStream.Flush();
            //    }
            //}
            //return RedirectToPage("index");
}