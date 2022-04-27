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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
               
        public Audio audio { get; set; }
                
        public IList<Audio> audios { get; set; }
       
        public InputModel Input { get; set; }
        
        public void OnGet()
        {
            Input = new InputModel();
            Input.lstCategories = _context.CategoriesTutos.ToList();
        }

        public class InputModel
        {            
            public IFormFile FormFileAudio { get; set; }
        
            public List<CategoriesTutos> lstCategories { get; set; }
        }
        
        public async Task<IActionResult> OnPost(string titre, string description, string categorie, IFormFile fileAudio, IFormFile fileImage)
        {
            try
            {
                string nomFichier = "";

                if (description == null)
                {
                    description = "aucune description disponible pour ce poadcast";
                }

                CategoriesTutos cat = _context.CategoriesTutos.Where(c => c.Nom == categorie).First();

                using (var client = new AmazonS3Client("AKIAVDH3AEDD6PUJMKGG", "kKV5WKu0tFe8Svl2QdTIMIydLc7CGSMiy2h+KOvV", RegionEndpoint.CACentral1))
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        fileAudio.CopyTo(newMemoryStream);
                        Debug.WriteLine("My debug string here");
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = fileAudio.FileName, // filename
                            BucketName = "mediafileobjectifresiliance", // bucket name of S3
                            CannedACL = S3CannedACL.PublicRead,
                        };

                        var fileTransferUtil = new TransferUtility(client);
                        await fileTransferUtil.UploadAsync(uploadRequest);
                    }

                    if (fileImage != null)
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            fileImage.CopyTo(newMemoryStream);
                            Debug.WriteLine("My debug string here");
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = fileImage.FileName, // filename
                                BucketName = "mediafileobjectifresiliance", // bucket name of S3
                                CannedACL = S3CannedACL.PublicRead,
                            };

                            var fileTransferUtil = new TransferUtility(client);
                            await fileTransferUtil.UploadAsync(uploadRequest);
                        }
                        nomFichier = fileImage.FileName;
                    }
                    else
                    {
                        nomFichier = "imagePlaceHolder.png";
                    }

                    Audio audio = new Audio
                    {
                        titre = titre,
                        urlAudio = "https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/" + fileAudio.FileName,
                        description = description,
                        categorie = cat,
                        urlImage = "https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/" + nomFichier,
                        EstPublier = false
                    };

                    _context.Audios.Add(audio);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("PoadcastPage");
                }
            
            }
            catch
            {
                return Page();
            }
        }
    }
    
}