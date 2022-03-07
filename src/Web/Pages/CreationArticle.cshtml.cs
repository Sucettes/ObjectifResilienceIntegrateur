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


        [HttpPost("FileUpload")]
        public async Task<IActionResult> OnPost(string titre, string inerText)
        {

            // Upload de l'article et sont titre 
            int idNewArticle = 0;
            Article newArticle = new Article
            {
                Titre = titre,
                InerText = inerText
            };
            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();
            idNewArticle = newArticle.Id;


            // mettre dans app setting
            using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
            {

                foreach (IFormFile File in Input.FormFile) {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            File.CopyTo(newMemoryStream);
                            Debug.WriteLine("My debug string here");
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = File.FileName, // filename
                                BucketName = "mediafileobjectifresiliance" // bucket name of S3
                            };

                            var fileTransferUtility = new TransferUtility(client);
                            await fileTransferUtility.UploadAsync(uploadRequest);
                        }

                    Media newMedia = new Media
                    {
                        Id = File.FileName,
                        Idreference = idNewArticle,
                        LinkS3 = "s3/mediafileobjectifresiliance/" + File.FileName,
                        OrderInThePage = 0
                    };
                    _context.Medias.Add(newMedia);
                    await _context.SaveChangesAsync();
                }
            }

            return Page();
        }

    }
}
//s3://mediafileobjectifresiliance/burger.jpg