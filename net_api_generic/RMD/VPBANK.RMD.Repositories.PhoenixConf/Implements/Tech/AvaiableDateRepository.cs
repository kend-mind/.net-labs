using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Tech;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.Tech
{
    public class ConfFilCeRepository : Repository<PhoenixConfContext, ConfFilCe, decimal>, IConfFilCeRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<ConfFilCe> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public ConfFilCeRepository(IDistributedCache distributedCache, ILogger<ConfFilCe> logger, ITrackableRepository<PhoenixConfContext, ConfFilCe, decimal> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
