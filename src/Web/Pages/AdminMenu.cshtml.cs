using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Web.FctUtils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

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
        private IList<NewPage> NewPages { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            NewPages = await _context.NewPages.ToListAsync();
            ViewData["NewPages"] = NewPages;

            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {

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
                            string champGuid = Request.Query["guid"];
                            string champRecherche = Request.Query["recherche"];
                            if (champGuid is not null)
                            {
                                Guid selectedUserId = Guid.Parse(Request.Query["guid"]);
                                if (DynamicQueryableExtensions.Any(_context.Roles))
                                {
                                    List<Role> lstRolesUser = Permission.ObtenirLstRolesUser(selectedUserId, _context);
                                    ViewData["userRoles"] = lstRolesUser;
                                    ViewData["selectRoles"] = ObtenirLstRolesSelect(lstRolesUser);
                                    User userSelected = _context.Users.Find(selectedUserId);
                                    ViewData["selectedUser"] = _context.Users.Find(selectedUserId);
                                }
                                else
                                {

                                    string[] strBidon = { "" };
                                    Role roleAdmin = new("Administrateur", strBidon, true);
                                    Role roleGDC = new("Gestionnaire de contenu", strBidon, true);
                                    Role roleUser = new("Utilisateur", strBidon, true);
                                    _context.Add(roleAdmin);
                                    _context.Add(roleGDC);
                                    _context.Add(roleUser);
                                    _context.SaveChanges();
                                    return Redirect("/AdminMenu/?tab=roles");
                                }
                            }
                            else if (champRecherche is not null)
                            {
                                List<User> usersFiltre = _context.Users.Where(u => u.Email.Contains(champRecherche) && u.Active == true).ToList();
                                ViewData["lstUsers"] = usersFiltre;
                                return Page();
                            }
                        }
                        else if (Tab == "utilisateurs")
                        {
                            string recherche = Request.Query["recherche"];
                            if (recherche != null)
                            {
                                List<User> userList = _context.Users.Where(u => u.Email.Contains(recherche)).ToList();
                                ViewData["tupleUsers"] = getListUserAndRoles(userList);
                                ViewData["Tab"] = "utilisateurs";
                                return Page();
                            }
                        }
                        else if (Tab == "demandes")
                        {
                            string recherche = Request.Query["recherche"];
                            if (recherche != null)
                            {
                                Users = await _context.Users.ToListAsync();
                                ViewData["lstUsers"] = Users;
                                UsersNonActivated = _context.Users.Where(u => u.Active == false && u.Email.Contains(recherche)).ToList();
                                if (UsersNonActivated.Count != 0)
                                {
                                    ViewData["lstNonActiver"] = UsersNonActivated;
                                }
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
                        if (Tab == "podcasts")
                        {

                            if (DynamicQueryableExtensions.Any(_context.Audios))
                            {
                                audios = await _context.Audios.ToListAsync();
                                ViewData["lstAudios"] = audios;
                            }
                        }
                        else if (Tab == "utilisateurs")
                        {
                            Users = await _context.Users.ToListAsync();
                            ViewData["tupleUsers"] = getListUserAndRoles((List<User>)Users);
                        }
                        else if (Tab == "demandes")
                        {
                            Users = await _context.Users.ToListAsync();
                            ViewData["lstUsers"] = Users;
                            UsersNonActivated = _context.Users.Where(u => u.Active == false).ToList();
                            if (UsersNonActivated.Count != 0)
                            {
                                ViewData["lstNonActiver"] = UsersNonActivated;
                            }
                        }
                        else if (Tab == "roles")
                        {
                            Users = await _context.Users.ToListAsync();
                            Users = Users.Where(u => u.Active == true).ToList();
                            ViewData["lstUsers"] = Users;
                        }
                        else if (Tab == "categories")
                        {
                            if (DynamicQueryableExtensions.Any(_context.CategoriesTutos))
                            {
                                lstCategories = _context.CategoriesTutos.ToList<CategoriesTutos>();
                                ViewData["lstCategories"] = lstCategories;
                            }
                        }
                        else if (Tab == "tutoriels")
                        {
                            if (DynamicQueryableExtensions.Any(_context.Tutos))
                            {
                                List<Tutos> lstTutos = (List<Tutos>)_context.Tutos.ToList();
                                ViewData["lstTutos"] = lstTutos;
                            }
                        }
                        else if(Tab == "pages")
                        {
                            if (DynamicQueryableExtensions.Any(_context.Articles))
                                ViewData["lstPages"] = _context.Articles.ToList();

                        }
                        else if (Tab == "articles")
                        {
                            if (DynamicQueryableExtensions.Any(_context.Articles))
                                ViewData["lstArticles"] = _context.Articles.ToList();
                        }
                        else
                        {
                            return Redirect("/adminMenu/?tab=utilisateurs");
                        }
                        return Page();
                    }
                }
            }
            return Redirect("/");
        }
        public List<(User, List<Role>)> getListUserAndRoles(List<User> pLstUser)
        {
            List<(User, List<Role>)> tupleUsers = new List<(User, List<Role>)>();
            foreach (User user in pLstUser)
            {
                tupleUsers.Add((user, Permission.ObtenirLstRolesUser(user.Id, _context)));
            }
            return tupleUsers;
        }
        public async Task<IActionResult> OnPostAsync(string btnSupprimerArticle, string btnSupprimer, string btnSupprimerTuto, string idAudio, string idTuto, string supprCatVal, string nomCat, string rechercheValeurDemande, string rechercheValeurUtilisateur, string rechercheValeurUtilisateurRole, string btnDeleteRole, string name, string selectRole, string btnAccepter, string btnRefuser, int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid idConnectedUser = ObtenirIdDuUserSelonEmail(User.Identity.Name);
                if (Permission.EstAdministrateur(idConnectedUser, _context))
                {
                    if (supprCatVal is not null)
                    {
                        Console.WriteLine(supprCatVal);
                        CategoriesTutos catTuto = (CategoriesTutos)_context.CategoriesTutos.Where(c => c.Id == Guid.Parse(supprCatVal)).First();
                        Console.WriteLine(catTuto.Nom);
                        _context.CategoriesTutos.Remove(catTuto);
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=categories");
                    }
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
                        User userBd = (User)_context.Users.Where(u => u.Id == Guid.Parse(name)).First();
                        userBd.Active = true;
                        _context.UserRoles.Add(new UserRole(userBd.Id, _context.Roles.Where(r => r.Name == "Utilisateur").First().Id));
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=demandes");
                    }
                    else if (btnRefuser is not null)
                    {
                        User userBd = (User)_context.Users.Where(u => u.Id == Guid.Parse(name)).First();
                        _context.Users.Remove(userBd);
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=demandes");
                    }
                    else if (rechercheValeurUtilisateurRole is not null)
                    {
                        return Redirect("/AdminMenu/?tab=roles&recherche=" + rechercheValeurUtilisateurRole);
                    }
                    else if (rechercheValeurUtilisateur is not null)
                    {
                        return Redirect("/AdminMenu/?tab=utilisateurs&recherche=" + rechercheValeurUtilisateur);
                    }
                    else if (rechercheValeurDemande is not null)
                    {
                        return Redirect("/AdminMenu/?tab=demandes&recherche=" + rechercheValeurDemande);
                    }
                    else if (nomCat != null)
                    {
                        CategoriesTutos cat = new();
                        cat.Nom = nomCat;
                        cat.Description = "";

                        if (!String.IsNullOrEmpty(nomCat) && nomCat.Length <= 50)
                        {
                            if (_context.CategoriesTutos.Where(c => c.Nom == nomCat).Count() == 0)
                            {
                                _context.CategoriesTutos.Add(cat);
                                await _context.SaveChangesAsync();
                            }
                        }
                        return Redirect("/AdminMenu/?tab=categories");
                    }
                    else if (idAudio != null)
                    {
                        Audio audio = new Audio();
                        try
                        {
                            audio = _context.Audios.Find(Guid.Parse(idAudio));
                        }
                        catch
                        {
                            Console.WriteLine("ERREUR");
                        }
                        if (audio != null)
                        {
                            if (audio.EstPublier)
                            {
                                audio.EstPublier = false;
                            }
                            else
                            {
                                audio.EstPublier = true;
                            }
                            _context.SaveChanges();
                            return Redirect("/AdminMenu/?tab=podcasts");
                        }
                    }
                    else if (idTuto != null)
                    {
                        Tutos tuto = new Tutos();
                        try
                        {
                            tuto = _context.Tutos.Find(Guid.Parse(idTuto));
                        }
                        catch
                        {
                            Console.WriteLine("ERREUR");
                        }
                        if(tuto != null)
                        {
                            if (tuto.EstPublier)
                            {
                                tuto.EstPublier = false;
                            }
                            else
                            {
                                tuto.EstPublier = true;
                            }
                            _context.SaveChanges();
                            return Redirect("/AdminMenu/?tab=tutoriels");
                        }
                       
                    }
                    else if (btnSupprimer is not null)
                    {
                        Audio audioBd = _context.Audios.Where(a => a.ID == Guid.Parse(name)).First();
                        _context.Audios.Remove(audioBd);
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=podcasts");
                    }
                    else if (btnSupprimerTuto is not null)
                    {
                        Tutos tutoBD = _context.Tutos.Where(t => t.Id == Guid.Parse(name)).First();
                        List<RangeeTutos> lstRangee = _context.RangeeTutos.Where(r => r.TutorielId == tutoBD.Id).ToList();
                        foreach(RangeeTutos range in lstRangee)
                        {
                            _context.RangeeTutos.Remove(range);
                        }
                        _context.Tutos.Remove(tutoBD);
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=tutoriels");
                    }
                    else if (btnSupprimerArticle is not null)
                    {
                        Article articleBD = _context.Articles.Where(a => a.Id == int.Parse(name)).First();
                        _context.Articles.Remove(articleBD);
                        await _context.SaveChangesAsync();
                        return Redirect("/AdminMenu/?tab=articles");
                    }
                }
            }
            return Redirect("/");
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
