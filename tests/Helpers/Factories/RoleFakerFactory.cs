using Bogus;
using Gwenael.Domain.Entities;
using System.Collections.Generic;

namespace Gwenael.Tests.Helpers.Factories
{
    public class RoleFakerFactory
    {
        public static Faker<Role> RoleFaker()
        {
            return new Faker<Role>()
                .RuleFor(r => r.Id, f => f.Random.Guid())
                .RuleFor(r => r.Name, f => f.Lorem.Word())
                .RuleFor(r => r.NormalizedName, (_, r) => r.Name.Normalize())
                .RuleFor(r => r.Users, (_, r) =>
                {
                    var user = UserFakerFactory.UserFaker().Generate();
                    return new List<UserRole>
                    {
                        new()
                        {
                            Role = r,
                            RoleId = r.Id,
                            User = user,
                            UserId = user.Id
                        }
                    };
                })
                .RuleFor(r => r.Claims, (f, r) => new List<RoleClaim>
                {
                    new()
                    {
                        Id = f.Random.Int(),
                        RoleId = r.Id,
                        ClaimType = "Permission",
                        ClaimValue = f.Lorem.Word()
                    }
                })
                .RuleFor(r => r.Active, f => f.Random.Bool())
                .RuleFor(r => r.ConcurrencyStamp, f => f.Random.Guid().ToString());
        }
    }
}