using System;
using System.Collections.Generic;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Authorization
{
    public static class SuperAdmins
    {
        public static IEnumerable<User> GetAllSuperAdmins()
        {
            var superAdmins = typeof(Constants.Authorization.SuperAdmins).GetFields();

            foreach (var field in superAdmins)
            {
                var superAdminEmail = field.GetValue(null) as string;
                yield return new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "geeks",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    Active = true,
                    FirstName = "Geeks",
                    LastName = "Spektrum",
                    PhoneNumber = "4188007440",
                    PhoneNumberConfirmed = true
                };
            }
        }
    }
}