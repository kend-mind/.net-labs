using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class ApproveStatusRepository : Repository<PhoenixConfContext, ApproveStatus, int>, IApproveStatusRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<ApproveStatus> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public ApproveStatusRepository(IDistributedCache distributedCache, ILogger<ApproveStatus> logger, ITrackableRepository<PhoenixConfContext, ApproveStatus, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }

        public IList<ApproveStatus> FindAllByFkRequestIdAndStatus(int reqId, string reqStatus)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.Approve_Status AS c WHERE c.Fk_Request_ID = {reqId} AND c.Status = '{reqStatus}'")
                .AsEnumerable()
                .ToList();
        }
    }
}
