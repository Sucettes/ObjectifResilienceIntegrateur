using AutoMapper;

namespace Gwenael.Application.Profiles
{
    public partial class Profiles : Profile
    {
        public Profiles()
        {
            CreateUserMap();
            CreateAccountMap();
            CreateRoleMap();
            CreatePagedListMap();
        }
    }
}