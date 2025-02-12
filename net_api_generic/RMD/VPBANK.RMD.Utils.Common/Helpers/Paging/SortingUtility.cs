using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VPBANK.RMD.Utils.Common.Extensions;
using static VPBANK.RMD.Utils.Common.Enums;

namespace VPBANK.RMD.Utils.Common.Helpers.Paging
{
    public class SortingUtility
    {
        public class SortingParams
        {
            public SortOrders SortOrder { get; set; } = SortOrders.Asc;
            public string ColumnName { get; set; }
        }

        /// <summary>  
        /// Enum for Sorting order  
        /// Asc = Ascending  
        /// Desc = Descending  
        /// </summary>  
        public class Sorting<T> where T : class
        {
            /// <summary>  
            /// Actual grouping will be done in ui,   
            /// from api we will send sorted data based on grouping columns  
            /// </summary>  
            /// <param name="data"></param>  
            /// <param name="groupingColumns"></param>  
            /// <returns></returns>  
            public static IEnumerable<T> GroupingData(IEnumerable<T> data, IEnumerable<string> groupingColumns)
            {
                IOrderedEnumerable<T> groupedData = null;

                foreach (string grpCol in groupingColumns.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (col != null)
                    {
                        groupedData = groupedData == null
                            ? data.OrderBy(x => col.GetValue(x, null))
                            : groupedData.ThenBy(x => col.GetValue(x, null));
                    }
                }

                return groupedData ?? data;
            }

            public static IEnumerable<T> SortData(IEnumerable<T> data, IEnumerable<SortingParams> sortingParams)
            {
                IOrderedEnumerable<T> sortedData = null;

                var currentSortingParams = sortingParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)).ToList();
                if (!currentSortingParams.Any())
                    currentSortingParams.Add(new SortingParams { ColumnName = "id", SortOrder = SortOrders.Asc });

                foreach (var sortingParam in currentSortingParams)
                {
                    sortingParam.ColumnName = sortingParam.ColumnName.ToPascalCaseWithUnderscore();
                    var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (col != null)
                        sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? data.OrderBy(x => col.GetValue(x, null))
                                                                                                   : data.OrderByDescending(x => col.GetValue(x, null))
                                                        : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(x => col.GetValue(x, null))
                                                                                                   : sortedData.ThenByDescending(x => col.GetValue(x, null));
                }
                return sortedData ?? data;
            }
        }
    }
}
