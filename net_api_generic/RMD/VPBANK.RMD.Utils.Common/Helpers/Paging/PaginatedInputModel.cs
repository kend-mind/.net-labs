using System.Collections.Generic;

namespace VPBANK.RMD.Utils.Common.Helpers.Paging
{
    /// <summary>  
    /// This class contains properites used for paging, sorting, grouping, filtering and will be used as a parameter model  
    ///   
    /// SortOrder       - Enum of sorting orders  
    /// SortColumn      - Name of the column on which sorting has to be done, as for now sorting can be performed only on one column at a time.  
    /// FilterParams    - Filtering can be done on multiple columns and for one column multiple values can be selected  
    ///                     Key :   - will be column name, 
    ///                     Value : - will be array list of multiple values  
    /// GroupingColumns - It will contain column names in a sequence on which grouping has been applied
    /// PageNumber      - Page Number to be displayed in UI, default to 1  
    /// PageSize        - Number of items per page, default to 25
    /// </summary>
    public class PaginatedInputModel
    {
        /// <summary>
        /// Sql condition filter
        /// </summary>
        public string FilterExpression { get; set; }

        public IEnumerable<FilterUtility.FilterParams> FilterParams { get; set; }

        public IEnumerable<SortingUtility.SortingParams> SortingParams { set; get; }

        public IEnumerable<string> GroupingColumns { get; set; } = null;

        private int pageIndex = 0;

        public int PageIndex
        {
            get { return pageIndex; }
            set { if (value > 0) pageIndex = value; }
        }

        private int pageSize = 25;

        public int PageSize
        {
            get { return pageSize; }
            set { if (value > 0) pageSize = value; }
        }
    }

    public static class QueryConditions
    {
        public const string SQL_STR_CONDITION = "(ISNULL(CHARINDEX(N\'{0}\', \"{1}\"), 0) > 0)";
    }
}
