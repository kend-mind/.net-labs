using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Params;
using VPBANK.RMD.Data.Collection.StoredProcedures;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Collection.Interfaces
{
    public interface ICollectionRepayReportRepository : IQueryRepository<CollectionContext, CollectionRepayReport>
    {
        public IEnumerable<CollectionRepayReport> GetCollectionRepayReport(CollectionRepayReportParam reportParam);
    }
}