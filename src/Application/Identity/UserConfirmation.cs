using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Gwenael.Domain.Entities;

namespace Gwenael.Application.Identity
{
    public class UserConfirmation : IUserConfirmation<User>
    {
        public Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(user.Active);
        }
    }
}