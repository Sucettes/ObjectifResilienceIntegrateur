using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gwenael.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gwenael.Domain.Stores
{
    public class GwenaelUserManager : UserManager<User>
    {
        public GwenaelUserManager(IUserStore<User> userStore, 
            IOptions<IdentityOptions> options, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<User>> logger) : base(userStore, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            
        }



        public override async Task<User> FindByNameAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            userName = NormalizeName(userName);

            return await Users
                .Include(x => x.Roles)
                .ThenInclude(r => r.Role)
                .ThenInclude(r => r.Claims)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == userName);
        }
    }
}
