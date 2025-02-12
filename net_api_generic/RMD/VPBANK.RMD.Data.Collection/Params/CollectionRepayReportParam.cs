using System;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Params
{
    public class CollectionRepayReportParam : BaseQueryResult
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CustomerId { get; set; }
        public string OsCompany { get; set; }
    }
}
