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
    public class PoadcastFormModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public PoadcastFormModel(GwenaelDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public Poadcast poadcast { get; set; }

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
        public async Task<IActionResult> OnPost(string titre, string description, string categorie)
        {
            // Upload de l'article et sont titre 
            //int idNewPoadcast = 0;
            //Poadcast newPoadcast = new Poadcast
            //{
                //titre = titre,
                //description = description,
                //categorie = categorie
            //};
            //_context.Articles.Add(newArticle);
            //_context.Poadcasts.Add(newPoadcast);
            //await _context.SaveChangesAsync();
            //idNewPoadcast = newPoadcast.ID;

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
                            BucketName = "mediafileobjectifresiliance" // bucket name of S3
                        };

                        var fileTransferUtil = new TransferUtility(client);
                        await fileTransferUtil.UploadAsync(uploadRequest);
                    }

                    Poadcast poadcast = new Poadcast
                    {

                        ID = File.FileName,
                        titre = titre,
                        url = "s3/mediafileobjectifresiliance/" + File.FileName,
                        description = description,
                        categorie = categorie
                    };


                    _context.Poadcasts.Add(poadcast);
                    await _context.SaveChangesAsync();
                    
                }
                return RedirectToPage("index");
            }
        }
    }
}