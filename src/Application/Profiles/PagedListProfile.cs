using Gwenael.Application.Dtos;
using Gwenael.Application.Extensions;

namespace Gwenael.Application.Profiles
{
    public partial class Profiles
    {
        private void CreatePagedListMap()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedListDto<>))
                .ForMember("Items", opt => opt.MapFrom(x => x))
                .ReverseMap();
        }
    }
}