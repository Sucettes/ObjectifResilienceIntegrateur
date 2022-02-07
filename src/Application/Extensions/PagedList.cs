using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Gwenael.Application.Extensions
{
    public class PagedList<TSource> : List<TSource>, IPagedList<TSource>, ISerializable where TSource : class
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;

        public PagedList()
        {
        }

        public PagedList(IEnumerable<TSource> source, int page, int size, int total) : this(source, page, size)
        {
            TotalItems = total;
        }

        public PagedList(IEnumerable<TSource> source, int page, int size)
        {
            PageSize = size < 0 ? int.MaxValue : size;
            CurrentPage = page < 0 ? 1 : page;
            TotalItems = source.Count();
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);

            var items = source
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            AddRange(items);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(PageSize), PageSize);
            info.AddValue(nameof(CurrentPage), CurrentPage);
            info.AddValue(nameof(TotalItems), TotalItems);
            info.AddValue(nameof(TotalPages), TotalPages);
            info.AddValue("Items", this);
        }
    }

    public static class PagedListExtensions
    {
        public static PagedList<TSource> ToPagedList<TSource>(this IEnumerable<TSource> query, int page = 1,
            int size = 25) where TSource : class
        {
            return new PagedList<TSource>(query, page, size);
        }
    }
}