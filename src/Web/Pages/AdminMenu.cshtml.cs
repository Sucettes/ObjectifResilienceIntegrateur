using Gwenael.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Gwenael.Domain;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;
using System.Linq;
using Gwenael.Web.FctUtils;
using Gwenael.Application.Mailing;

namespace Gwenael.Web.Pages
{
    public class AdminMenuModel : PageModel
    {
        private UserManager<User> _userManager;
        public AdminMenuModel(GwenaelDbContext context, UserManager<User> pUserManager)
        {
            _context = context;
            _userManager = pUserManager;
        }
        public IList<User> Users { get; set; }
        public GwenaelDbContext _context { get; set; }
        [BindProperty]
        public IList<User> UsersNonActivated { get; set; }
        public IList<Audio> audios { get; set; }
        public List<CategoriesTutos> lstCategories { get; set; }
        public String Tab { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
            //    if (Permission.EstAdministrateur(idConnectedUser, _context))
            //    {
            if (DynamicQueryableExtensions.Any(_context.CategoriesTutos))
            {
                lstCategories = _context.CategoriesTutos.ToList<CategoriesTutos>();
            }
            if (DynamicQueryableExtensions.Any(_context.Audios))
            {
                audios = await _context.Audios.ToListAsync();
                ViewData["lstAudios"] = audios;
            }

           

            if (Request.Query.Count == 1)
            {
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
            }
            else if (Request.Query.Count > 1)
            {
                Tab = Request.Query["tab"];
                ViewData["Tab"] = Tab;
                if (Tab == "roles")
                {
                    Guid selectedUserId = Guid.Parse(Request.Query["guid"]);
                    if (DynamicQueryableExtensions.Any(_context.Roles))
                    {
                        List<Role> lstRolesUser = Permission.ObtenirLstRolesUser(selectedUserId, _context);
                        ViewData["userRoles"] = lstRolesUser;
                        ViewData["selectRoles"] = ObtenirLstRolesSelect(lstRolesUser);
                    }
                    else
                    {

                        string[] jjj = { "" };
                        Role roleAdmin = new("Administrateur", jjj, true);
                        Role roleGDC = new("Gestionnaire de contenu", jjj, true);
                        Role roleUser = new("Utilisateur", jjj, true);
                        _context.Add(roleAdmin);
                        _context.Add(roleGDC);
                        _context.Add(roleUser);
                        _context.SaveChanges();
                    }
                }
                else if (Tab == "utilisateurs")
                {
                    string recherche = Request.Query["recherche"];
                    if (recherche != null)
                    {
                        List<User> userList = _context.Users.Where(u => u.Email.Contains(recherche)).ToList();
                        ViewData["lstUsers"] = userList;
                        ViewData["tupleUsers"] = getListUserAndRoles(userList);
                        ViewData["Tab"] = "utilisateurs";
                        return Page();
                    }
                   
                }
                string erreur = Request.Query["error"];
                if (erreur != null)
                {
                    ViewData["msgErreur"] = "Vous ne pouvez pas retirer votre accès administrateur";
                }
            }
            if (Tab == null || Tab == "")
            {
                ViewData["Tab"] = "utilisateurs";
                return Redirect("/AdminMenu/?tab=utilisateurs");
            }
            else
            {
                Users = await _context.Users.ToListAsync();
                ViewData["lstUsers"] = Users;
                ViewData["tupleUsers"] = getListUserAndRoles((List<User>)Users);
                UsersNonActivated = _context.Users.Where(u => u.Active == false).ToList();
                ViewData["lstNonActiver"] = UsersNonActivated;
                return Page();
            }
            //}
            //    }
            //    return Redirect("/");
            //}
        }
        public List<(User,List<Role>)> getListUserAndRoles(List<User> pLstUser)
        {
            List<(User, List<Role>)> tupleUsers = new List<(User, List<Role>)>();
            foreach (User user in pLstUser)
            {
                tupleUsers.Add((user, Permission.ObtenirLstRolesUser(user.Id, _context)));
            }
            return tupleUsers;
        }
        public async Task<IActionResult> OnPostAsync(string rechercheValeurUtilisateur, string btnDeleteRole, string name, string selectRole, string btnAccepter, int? id)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
            //if (Permission.EstAdministrateur(idConnectedUser, _context))
            //{
            

            if (btnDeleteRole is not null)
            {
                Guid selectedUserId = Guid.Parse(Request.Query["guid"]);
                Guid idRole = ObtenirIdDuRoleSelonNom(name);

                UserRole userRole = ObtenirUserRole(selectedUserId, idRole);
                if (userRole != null)
                {
                    if (name == "Administrateur" && selectedUserId == ObtenirIdDuUserSelonEmail(User.Identity.Name))
                    {
                        return Redirect("/AdminMenu/?tab=roles&guid=" + selectedUserId + "&error=true");
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
            else if (btnAccepter is not null)
            {
                //// Modification Active à true
                User userBd = (User)_context.Users.Where(u => u.Id == Guid.Parse(name)).First();
                userBd.Active = true;
                _context.UserRoles.Add(new UserRole(userBd.Id,_context.Roles.Where(r => r.Name == "Utilisateur").First().Id));
                await _context.SaveChangesAsync();
                return Redirect("/AdminMenu/?tab=demandes");
            }
            else if (rechercheValeurUtilisateur is not null)
            {
                return Redirect("/AdminMenu/?tab=utilisateurs&recherche="+rechercheValeurUtilisateur);
            }
            else
            {
                return Redirect("/AdminMenu/?tab=utilisateurs&recherche=");
            }
            //}
            //}
            //return Redirect("/");
        }

        public Guid ObtenirIdDuUserSelonEmail(string email)
        {
            User user = (User)_context.Users.Where(u => u.UserName == email).First();
            return user.Id;
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
    }
}
