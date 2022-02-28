using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gwenael.Web.Pages
{
    public class CreationArticle : PageModel
    {
        public string PersonalDetails { get; set; }

        public void OnGet()
        {

        }

        public void OnPostSubmit(string personalDetails)
        {
            this.PersonalDetails = personalDetails;
            
        }
    }
}
