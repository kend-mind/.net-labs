using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class NotificationCountRepository : Repository<PhoenixConfContext, NotificationCount, int>, INotificationCountRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly PhoenixConfContext _context;

        public NotificationCountRepository(IDistributedCache distributedCache, ITrackableRepository<PhoenixConfContext, NotificationCount, int> trackableRepository,
            PhoenixConfContext context) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public NotificationCount FindFirstByUsername(string username)
        {
            return TrackableRepository.QueryableSql($"SELECT c.* FROM App.Notification_Count AS c WHERE c.Username = '{username}'").FirstOrDefault();
        }
    }
}
