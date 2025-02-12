using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPBANK.RMD.Utils.Common.Helpers.Paging
{
    public class PaginatedResults<T> : List<T>
    {
        public int TotalPages { get; private set; }
        public int TotalElements { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int NumberOfElements { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public bool First => PageIndex == 1;
        public bool Last => PageIndex == TotalPages;
        public bool Empty { get; private set; }

        public PaginatedResults(List<T> itemsOnPage, int totalElements, int pageIndex, int pageSize)
        {
            TotalElements = totalElements;
            TotalPages = (int)Math.Ceiling(totalElements / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
            NumberOfElements = itemsOnPage.Any() ? itemsOnPage.Count : 0;
            Empty = !itemsOnPage.Any();

            this.AddRange(itemsOnPage);
        }

        public static async Task<PaginatedResults<T>> QueryDataAsync(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var totalElements = source.Count();
            var itemsOnPage = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var task = await Task.Run(() => new PaginatedResults<T>(itemsOnPage, totalElements, pageIndex, pageSize));
            return task;
        }

        public static async Task<PaginatedResults<T>> QueryDataAsync(IEnumerable<T> source, int pageIndex, int pageSize, int total)
        {
            var totalElements = total;
            var itemsOnPage = source.ToList();
            var task = await Task.Run(() => new PaginatedResults<T>(itemsOnPage, totalElements, pageIndex, pageSize));
            return task;
        }
    }

    public class PaginatedContentResults<T> where T : class
    {
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public int NumberOfElements { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
        public bool Empty { get; set; }
        public PaginatedResults<T> Content { get; set; }
    }
}
