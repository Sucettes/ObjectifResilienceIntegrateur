using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gwenael.Domain.Entities;

namespace Gwenael.Application.Services
{
    public interface IUserService
    {
        public IEnumerable<User> Search(string sortBy, bool sortDesc, params Expression<Func<User, bool>>[] predicates);
    }
}