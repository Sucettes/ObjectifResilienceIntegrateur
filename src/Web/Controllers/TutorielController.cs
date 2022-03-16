using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.Controllers
{
    public class TutorielController : Controller
    {
        public IActionResult Index()
        {
            // Tutoriel -> tutoriel/hometuto
            //return RedirectToAction("HomeTuto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoToCreationTuto()
        {
            return RedirectToAction("CreationTuto");
        }

        [HttpPost]
        public async Task<IActionResult> BackToIndexTuto()
        {
            return RedirectToAction("Index");
        }
    }
}
