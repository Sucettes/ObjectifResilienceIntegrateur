using Bogus;
using Gwenael.Domain.Entities;

namespace Gwenael.Tests.Helpers.Factories
{
    public class UserFakerFactory
    {
        public static Faker<User> UserFaker()
        {
            return new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Active, f => f.Random.Bool())
                .RuleFor(u => u.CreationDate, f => f.Date.Past())
                .RuleFor(u => u.LastUpdateDate, f => f.Date.Recent())
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName.Normalize())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.Normalize())
                .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(u => u.PasswordHash, f => f.Random.Hash())
                .RuleFor(u => u.SecurityStamp, f => f.Random.Hash())
                .RuleFor(u => u.ConcurrencyStamp, f => f.Random.Guid().ToString())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.PhoneNumberConfirmed, f => f.Random.Bool())
                .RuleFor(u => u.TwoFactorEnabled, f => f.Random.Bool())
                .RuleFor(u => u.LockoutEnd, f => f.Date.FutureOffset())
                .RuleFor(u => u.LockoutEnabled, f => f.Random.Bool())
                .RuleFor(u => u.AccessFailedCount, f => f.Random.Number(3));
        }
    }
}