using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;

namespace Gwenael.Web.Pages
{
    public class AdminMenuModel : PageModel
    {
        private readonly GwenaelDbContext _context;

        public AdminMenuModel(GwenaelDbContext context)
        {
            _context = context;
            // Ajout Manuel d'un user
            //User antho = new User
            //{
            //    Active = true,
            //    FirstName = "Anthony",
            //    LastName = "Levesque",
            //    Email = "antho20k@hotmail.com"
            //};
            //_context.Add<User>(antho);
            //_context.SaveChanges();
        }
        public IList<User> Users { get; set; }
        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
            ViewData["lstUsers"] = Users;
        }
        public void OnPost() {
            
        }

    }
}
