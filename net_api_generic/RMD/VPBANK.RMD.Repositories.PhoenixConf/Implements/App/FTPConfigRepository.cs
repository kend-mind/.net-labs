using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class FTPConfigRepository : Repository<PhoenixConfContext, FTPConfig, int>, IFTPConfigRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<FTPConfig> _logger;
        protected readonly PhoenixConfContext _phoenixConfContext;

        public FTPConfigRepository(IDistributedCache distributedCache, ILogger<FTPConfig> logger, ITrackableRepository<PhoenixConfContext, FTPConfig, int> trackableRepository,
            PhoenixConfContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixConfContext = phoenixConfContext;
        }

        public FTPConfig FindByTargetServer(string targetServer)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.FTP_Config AS c WHERE c.Source_System = '{targetServer}'")
                .AsEnumerable()
                .FirstOrDefault();
        }
    }
}
