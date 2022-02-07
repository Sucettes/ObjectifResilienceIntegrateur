using System;
using System.Collections.Generic;
using Bogus;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Fakers
{
    public sealed class UserFaker : Faker<User>
    {
        public static UserFaker DefaultInstance() => new();

        private UserFaker()
        {
            RuleFor(u => u.Id, Guid.NewGuid);
            RuleFor(u => u.Email, f => f.Internet.Email());
            RuleFor(u => u.UserName, (f, u) => f.Random.AlphaNumeric(3));
            RuleFor(u => u.FirstName, f => f.Person.FirstName);
            RuleFor(u => u.LastName, f => f.Person.LastName);
            RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());
            RuleFor(u => u.EmailConfirmed, false);
            RuleFor(u => u.PhoneNumberConfirmed, true);
            RuleFor(u => u.TwoFactorEnabled, false);
            RuleFor(u => u.LockoutEnabled, false);
            RuleFor(u => u.AccessFailedCount, 0);
            RuleFor(u => u.Active, f => f.Random.Bool(.90f));
        }
    }
}