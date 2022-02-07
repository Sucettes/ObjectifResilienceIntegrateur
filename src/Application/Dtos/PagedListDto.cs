using System.Collections.Generic;

namespace Gwenael.Application.Dtos
{
    public class PagedListDto<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }

        public PagedListDto()
        {
        }
    }
}