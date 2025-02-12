using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.WebCore;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.WebCore;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.WebCore
{
    public class ConfTableMappingRepository : Repository<PhoenixConfContext, ConfTableMapping, int>, IConfTableMappingRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<ConfTableMapping> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public ConfTableMappingRepository(IDistributedCache distributedCache, ILogger<ConfTableMapping> logger, ITrackableRepository<PhoenixConfContext, ConfTableMapping, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
