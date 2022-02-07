using System;
using Gwenael.Application.Dtos;
using Gwenael.Application.Profiles.ValueConverters;
using Gwenael.Domain.Entities;

namespace Gwenael.Application.Profiles
{
    public partial class Profiles
    {
        private void CreateUserMap()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Id, opt => opt.Condition((src) => src.Id != Guid.Empty))
                .ForMember(x => x.IsLockedOut,
                    opt => opt.ConvertUsing(new DateTimeOffsetBoolConverter(), src => src.LockoutEnd));

            CreateMap<UserDto, User>()
                .ForMember(x => x.Id, opt => opt.Condition((src) => src.Id != Guid.Empty))
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.LockoutEnd,
                    opt => opt.ConvertUsing(new BoolDateTimeOffsetConverter(), dto => dto.IsLockedOut))
                .ForMember(x => x.CreationDate, opt => opt.Ignore())
                .ForMember(x => x.LastUpdateDate, opt => opt.Ignore())
                .ForMember(x => x.EmailConfirmed, opt => opt.Ignore())
                .ForMember(x => x.NormalizedEmail, opt => opt.Ignore())
                .ForMember(x => x.NormalizedUserName, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore())
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore())
                .ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(x => x.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(x => x.LockoutEnabled, opt => opt.Ignore())
                .ForMember(x => x.AccessFailedCount, opt => opt.Ignore());
        }
    }
}