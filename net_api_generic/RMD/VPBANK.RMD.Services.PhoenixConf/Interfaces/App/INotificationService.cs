using System.Collections.Generic;
using System.Threading.Tasks;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;

namespace VPBANK.RMD.Services.PhoenixConf.Interfaces.App
{
    public interface INotificationService
    {
        int GetNotiCount(string username);
        Task ResetNotiCountAsync(string username);
        void IncrementCount(string username);
        void IncrementCount(List<string> usernames);
        Task IncrementCountAsync(string username);

        Task AddSubscriberAsync(string username, string routingKey);
        Task RemoveSubscriberAsync(string username);
        Task RemoveSubscriberAsync(string username, string routingKey);

        List<string> FindAllRoutingKeyByUsername(string username);
        List<string> FindAllSubscribersByRoutekey(string routingKey);
        bool IsMatchSubscribe(SubscriberInfo subscriberInfo);
    }
}
