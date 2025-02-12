using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class FTPDocumentTypeRepository : Repository<PhoenixConfContext, FTPDocumentType, int>, IFTPDocumentTypeRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<FTPDocumentType> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public FTPDocumentTypeRepository(IDistributedCache distributedCache, ILogger<FTPDocumentType> logger, ITrackableRepository<PhoenixConfContext, FTPDocumentType, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }
    }
}
