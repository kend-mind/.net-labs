using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class RequestObjectRepository : Repository<PhoenixConfContext, RequestObject, int>, IRequestObjectRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<RequestObject> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public RequestObjectRepository(IDistributedCache distributedCache, ILogger<RequestObject> logger, ITrackableRepository<PhoenixConfContext, RequestObject, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
