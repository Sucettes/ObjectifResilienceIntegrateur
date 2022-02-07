using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Binbin.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Spk.Common.Helpers.Guard;

namespace Gwenael.Application.Services
{
    public class UserService : IUserService
    {
        private readonly GwenaelDbContext _context;

        public UserService(GwenaelDbContext context)
        {
            _context = context.GuardIsNotNull(nameof(context));
        }

        public IEnumerable<User> Search(string sortBy, bool sortDesc, params Expression<Func<User, bool>>[] expressions)
        {
            sortBy ??= "CreationDate";

            var query = _context.Users.Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.Claims)
                .AsNoTracking()
                .AsSplitQuery();

            var predicate = PredicateBuilder.False<User>();
            predicate = expressions
                .Aggregate(predicate, (current, expression) =>
                    current.Or(expression));

            return query.Where(predicate)
                .OrderBy($"{sortBy} {(sortDesc ? "desc" : "asc")}"); ;
        }
    }
}