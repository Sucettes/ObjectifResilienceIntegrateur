using Gwenael.Application.Dtos;
using Gwenael.Domain.Entities;

namespace Gwenael.Application.Profiles
{
    public partial class Profiles
    {
        private void CreateRoleMap()
        {
            CreateMap<Role, RoleDto>()
                .ReverseMap();
        }
    }
}