using System;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Params
{
    public class CollectionNewbookReportParam : BaseQueryResult
    {
        public DateTime ReportDate { get; set; }
        public string CustomerId { get; set; }
        public string OsCompany { get; set; }
    }
}
