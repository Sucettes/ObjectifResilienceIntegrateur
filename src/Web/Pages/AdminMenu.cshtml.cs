using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace Gwenael.Web.Pages
{
    public class AdminMenuModel : PageModel
    {
        private UserManager<User> _userManager;
        private readonly GwenaelDbContext _context;
        // Curent User
        //Guid _selectedUserId = _userManager.FindByEmailAsync(_userManager.GetUserName(User)).Result.Id;
        public AdminMenuModel(GwenaelDbContext context, UserManager<User> pUserManager)
        {
            _context = context;
            _userManager = pUserManager;

        }
        public IList<User> Users { get; set; }
        [BindProperty(SupportsGet = true)]
        public Role Role { get; set; }
        [BindProperty]
        public IList<Formation> Formations { get; set; }
        public String Tab { get; set; }
        [BindProperty]
        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Query.Count == 1)
            {
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
            }
            else if (Request.Query.Count == 2)
            {
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
                if (Tab == "roles")
                {
                    Guid selectedUserId = Guid.Parse(Request.Query["guid"]);
                    if (DynamicQueryableExtensions.Any(_context.Roles))
                    {
                        List<Role> lstRolesUser = ObtenirLstRolesUser(selectedUserId);
                        ViewData["userRoles"] = lstRolesUser;
                        ViewData["selectRoles"] = ObtenirLstRolesSelect(lstRolesUser);
                    }
                }
            }
            Users = await _context.Users.ToListAsync();
            ViewData["lstUsers"] = Users;
            Formations = await _context.Formations.ToListAsync();
            ViewData["lstFormations"] = Formations;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string btnDeleteRole, string name, string selectRole)
        {
            if(btnDeleteRole is not null)
            {
                Guid selectedUserId = Guid.Parse(Request.Query["guid"]);
                Guid idRole = ObtenirIdDuRoleSelonNom(name);

                UserRole userRole = ObtenirUserRole(selectedUserId, idRole);
                if (userRole != null)
                {
                    if (name == "Administrateur")
                    {

                    }
                    else if (name == "Gestionnaire de contenu")
                    {

                    }
                    else if (name == "Utilisateur")
                    {

                    }
                    _context.UserRoles.Remove(userRole);
                }
                await _context.SaveChangesAsync();
                return Redirect("/AdminMenu/?tab=roles&guid=" + selectedUserId);
            }
            else if (selectRole is not null)
            {
                Guid selectedUserId = Guid.Parse(Request.Query["guid"]);

                Role selectedRole = null;
                if (selectRole == "Administrateur")
                    selectedRole = _context.Roles.Where(r => r.Name == "Administrateur").First();
                else if (selectRole == "Gestionnaire de contenu")
                    selectedRole = _context.Roles.Where(r => r.Name == "Gestionnaire de contenu").First();
                else if (selectRole == "Utilisateur")
                    selectedRole = _context.Roles.Where(r => r.Name == "Utilisateur").First();
                else
                    return Redirect("/AdminMenu/?tab=roles&guid=" + selectedUserId);
                _context.UserRoles.Add(new UserRole(selectedUserId, selectedRole.Id));
                await _context.SaveChangesAsync();
                return Redirect("/AdminMenu/?tab=roles&guid=" + selectedUserId);
            }
            //var role = new Role("Administrateur", new[] { "Administrateur" });
            //var role2 = new Role("Modérateur", new[] { "Modérateur" });
            //var role3 = new Role("Gestionnaire de contenu", new[] { "Gestionnaire de contenu" });
            //var role4 = new Role("Utilisateur", new[] { "Utilisateur" });


            //_context.Roles.Add(role);
            //_context.Roles.Add(role2);
            //_context.Roles.Add(role3);
            //_context.Roles.Add(role4);

            //Console.WriteLine(User.Identity.Name);
            //_context.UserRoles.Add(new UserRole(_userManager.FindByEmailAsync(_userManager.GetUserName(User)).Result.Id,
            //    role.Id));

            return Page();
        }
        public UserRole ObtenirUserRole(Guid userId, Guid roleId)
        {
            try
            {
                return (UserRole)_context.UserRoles.Where(usr => usr.UserId == userId && usr.RoleId == roleId).First();
            }
            catch
            {
                return null;
            }
        }
        public Guid ObtenirIdDuRoleSelonNom(string nomDuRole)
        {
            Role role = (Role)_context.Roles.Where(r => r.Name == nomDuRole).First();
            return role.Id;
        }
        public List<Role> ObtenirLstRolesSelect(List<Role> lstRolesUser)
        {
            List<Role> lstRoleSelect = new List<Role>();
            if (lstRolesUser.Count != 0)
            {
                bool contientAdmin = false;
                bool contientGdC = false;
                bool contientUtilisateur = false;
                foreach (var roleUser in lstRolesUser)
                {
                    if (roleUser.Name == "Administrateur")
                    {
                        contientAdmin = true;
                    }
                    else if (roleUser.Name == "Gestionnaire de contenu")
                    {
                        contientGdC = true;
                    }
                    else if (roleUser.Name == "Utilisateur")
                    {
                        contientUtilisateur = true;
                    }
                }
                if (!contientAdmin)
                    lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Administrateur").First());
                if (!contientGdC)
                    lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Gestionnaire de contenu").First());
                if (!contientUtilisateur)
                    lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Utilisateur").First());
                return lstRoleSelect;
            }
            lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Administrateur").First());
            lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Gestionnaire de contenu").First());
            lstRoleSelect.Add((Role)_context.Roles.Where(r => r.Name == "Utilisateur").First());
            return lstRoleSelect;
        }
        public List<Role> ObtenirLstRolesUser(Guid userId)
        {
            List<UserRole> lstUserRolesBd = new List<UserRole>();
            List<Role> lstRolesBd = new List<Role>();

            List<UserRole> lstUserRoleDuUser = new List<UserRole>();
            List<Role> lstRolesDuUser = new List<Role>();

            lstRolesBd = _context.Roles.ToList();
            lstUserRolesBd = _context.UserRoles.ToList();
            foreach (var roleInUserRole in lstUserRolesBd)
            {
                if (roleInUserRole.UserId == userId)
                    lstUserRoleDuUser.Add(roleInUserRole);
            }
            foreach (var rolee in lstUserRoleDuUser)
            {
                foreach (var roleId in lstRolesBd)
                {
                    if (rolee.RoleId == roleId.Id)
                    {
                        lstRolesDuUser.Add(roleId);
                    }
                }
            }
            return lstRolesDuUser;
        }
    }
}
