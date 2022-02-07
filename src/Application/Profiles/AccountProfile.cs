using System;
using Gwenael.Application.Dtos;
using Gwenael.Application.Profiles.ValueConverters;
using Gwenael.Domain.Entities;

namespace Gwenael.Application.Profiles
{
    public partial class Profiles
    {
        private void CreateAccountMap()
        {
            CreateMap<User, AccountDto>()
                .ForMember(x => x.Id, opt => opt.Condition((src) => src.Id != Guid.Empty))
                .ForMember(x => x.IsLockedOut,
                    opt => opt.ConvertUsing(new DateTimeOffsetBoolConverter(), user => user.LockoutEnd));
        }
    }
}