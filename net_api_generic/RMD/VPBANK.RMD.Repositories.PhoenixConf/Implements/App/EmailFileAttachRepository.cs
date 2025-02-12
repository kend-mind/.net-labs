using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class EmailFileAttachRepository : Repository<PhoenixConfContext, EmailFileAttach, int>, IEmailFileAttachRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<EmailFileAttach> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public EmailFileAttachRepository(IDistributedCache distributedCache, ILogger<EmailFileAttach> logger, ITrackableRepository<PhoenixConfContext, EmailFileAttach, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
