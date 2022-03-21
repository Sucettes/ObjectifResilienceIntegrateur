using Gwenael.Domain;
using Gwenael.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gwenael.Web.FctUtils
{
    public static class Permission
    {
        public static bool VerifierAccesUtilisateur(Guid idUser, GwenaelDbContext context)
        {
            if (EstUtilisateur(idUser, context) || EstGestionnaireDeContenu(idUser, context) || EstAdministrateur(idUser, context))
            {
                return true;
            }
            return false;
        }
        public static bool VerifierAccesGdC(Guid idUser, GwenaelDbContext context)
        {
            if (EstGestionnaireDeContenu(idUser,context) || EstAdministrateur(idUser, context))
            {
                return true;
            }
            return false;
        }
        public static bool EstGestionnaireDeContenu(Guid idUser, GwenaelDbContext context)
        {
            List<Role> lstRole = ObtenirLstRolesUser(idUser, context);
            bool contientGdC= false;
            foreach (var role in lstRole)
            {
                if (role.Name == "Gestionnaire de contenu")
                {
                    contientGdC = true;
                }
            }
            return contientGdC;
        }
        public static bool EstUtilisateur(Guid idUser, GwenaelDbContext context)
        {
            List<Role> lstRole = ObtenirLstRolesUser(idUser, context);
            bool contientUtilisateur = false;
            foreach (var role in lstRole)
            {
                if (role.Name == "Utilisateur")
                {
                    contientUtilisateur = true;
                }
            }
            return contientUtilisateur;
        }
        public static bool EstAdministrateur(Guid idUser, GwenaelDbContext context)
        {
            List<Role> lstRole = ObtenirLstRolesUser(idUser, context);
            bool contientAdmin = false;
            foreach (var role in lstRole)
            {
                if (role.Name == "Administrateur")
                {
                    contientAdmin = true;
                }
            }
            return contientAdmin;
        }
        public static List<Role> ObtenirLstRolesUser(Guid userId, GwenaelDbContext _context)
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
