using Microsoft.Extensions.Caching.Distributed;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class EmailTemptRepository : Repository<PhoenixConfContext, EmailTempt, int>, IEmailTemptRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public EmailTemptRepository(ITrackableRepository<PhoenixConfContext, EmailTempt, int> trackableRepository, IDistributedCache distributedCache, 
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
