using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class SubscriberInfoRepository : Repository<PhoenixConfContext, SubscriberInfo, decimal>, ISubscriberInfoRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly PhoenixConfContext _context;

        public SubscriberInfoRepository(IDistributedCache distributedCache, ITrackableRepository<PhoenixConfContext, SubscriberInfo, decimal> trackableRepository,
            PhoenixConfContext context) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public IList<SubscriberInfo> FindAllSubscriberByUsername(string username)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.Subscriber_Info AS c WHERE c.Username = '{username}'")
                .AsEnumerable()
                .ToList();
        }

        public IList<SubscriberInfo> FindAllSubscriberByRoutingKey(string routingKey)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.Subscriber_Info AS c WHERE c.Routing_Key = '{routingKey}'")
                .AsEnumerable()
                .ToList();
        }

        public IList<SubscriberInfo> FindAllSubscriberByUsernameAndRoutingKey(string username, string routingKey)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.Subscriber_Info AS c WHERE c.Username = '{username}' c.Routing_Key = '{routingKey}'")
                .AsEnumerable()
                .ToList();
        }

        public int CountByUsernameAndRoutingKey(string username, string routingKey)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM App.Subscriber_Info AS c WHERE c.Username = '{username}' c.Routing_Key = '{routingKey}'")
                .AsEnumerable()
                .Count();
        }
    }
}
