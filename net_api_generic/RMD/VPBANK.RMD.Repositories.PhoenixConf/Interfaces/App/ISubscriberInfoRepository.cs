using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App
{
    public interface ISubscriberInfoRepository : IRepository<PhoenixConfContext, SubscriberInfo, decimal>
    {
        IList<SubscriberInfo> FindAllSubscriberByUsername(string username);
        IList<SubscriberInfo> FindAllSubscriberByRoutingKey(string routingKey);
        IList<SubscriberInfo> FindAllSubscriberByUsernameAndRoutingKey(string username, string routingKey);
        int CountByUsernameAndRoutingKey(string username, string routingKey);
    }
}
