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
    public class CollectionRepayReportRepository : QueryRepository<CollectionContext, CollectionRepayReport>, ICollectionRepayReportRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<CollectionRepayReport> _logger;
        protected readonly CollectionContext _collectionContext;

        public CollectionRepayReportRepository(IDistributedCache distributedCache,
            ILogger<CollectionRepayReport> logger,
            IQueryableRepository<CollectionContext, CollectionRepayReport> queryableRepository,
            CollectionContext collectionContext) : base(queryableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _collectionContext = collectionContext;
        }

        public IEnumerable<CollectionRepayReport> GetCollectionRepayReport(CollectionRepayReportParam reportParam)
        {
            return QueryableRepository.QueryableSql(string.Format(CollectionSqlQuery.get_Collection_Repay_Report, reportParam.FromDate, reportParam.ToDate, string.IsNullOrEmpty(reportParam.CustomerId) ? "null" : $"'{reportParam.CustomerId}'", reportParam.OsCompany));
        }
    }
}
