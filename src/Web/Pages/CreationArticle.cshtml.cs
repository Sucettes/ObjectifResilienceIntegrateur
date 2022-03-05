using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Gwenael.Web.Pages
{
    public class CreationArticle : PageModel
    {
        //public string InerText { get; set; }
        //public string Titre { get; set; }

        private readonly GwenaelDbContext _context;


        public CreationArticle(GwenaelDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]

        public Article article { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string titre, string inerText)
        {

            // Upload de l'article en html avec sont titre 
            Article newArticle = new Article
            {
                Titre = titre,
                InerText = inerText
            };
            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();

            
            // mettre dans app setting
            using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
            {

                using (var newMemoryStream = new MemoryStream())
                {
                    Upload.CopyTo(newMemoryStream);
                    Debug.WriteLine("My debug string here");
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = Upload.FileName, // filename
                        BucketName = "mediafileobjectifresiliance" // bucket name of S3
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }


            return Page();
        }

    }
}
