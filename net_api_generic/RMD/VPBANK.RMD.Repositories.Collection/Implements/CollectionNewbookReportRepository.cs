using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Params;
using VPBANK.RMD.Data.Collection.StoredProcedures;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class CollectionNewbookReportRepository : QueryRepository<CollectionContext, CollectionNewbookReport>, ICollectionNewbookReportRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<CollectionNewbookReport> _logger;
        protected readonly CollectionContext _collectionContext;

        public CollectionNewbookReportRepository(IDistributedCache distributedCache,
            ILogger<CollectionNewbookReport> logger,
            IQueryableRepository<CollectionContext, CollectionNewbookReport> queryableRepository,
            CollectionContext collectionContext) : base(queryableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _collectionContext = collectionContext;
        }

        public IEnumerable<CollectionNewbookReport> GetCollectionNewbookReport(CollectionNewbookReportParam reportParam)
        {
            return QueryableRepository.QueryableSql(string.Format(CollectionSqlQuery.get_Collection_NewBook_Report, reportParam.ReportDate, string.IsNullOrEmpty(reportParam.CustomerId) ? "null" : $"'{reportParam.CustomerId}'", reportParam.OsCompany));
        }
    }
}
