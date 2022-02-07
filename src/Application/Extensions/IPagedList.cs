namespace Gwenael.Application.Extensions
{
    public interface IPagedList<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }
    }
}