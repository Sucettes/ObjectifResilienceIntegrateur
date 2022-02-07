using System;
using System.Collections.Generic;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Authorization
{
    public static class Roles
    {
        public static IEnumerable<Role> GetAllRoles()
        {
            var roles = typeof(Constants.Authorization.Roles).GetFields();

            foreach (var field in roles)
            {
                var roleName = field.GetValue(null) as string;
                var id = Guid.NewGuid();
                if (roleName == Constants.Authorization.Roles.SUPER_ADMIN)
                {
                    id = Guid.Parse("4B93FE7F-A48A-4F60-BEA4-54F5B1E77315");
                }
                yield return new Role
                {
                    Id = id,
                    Name = roleName
                };
            }
        }
    }
}