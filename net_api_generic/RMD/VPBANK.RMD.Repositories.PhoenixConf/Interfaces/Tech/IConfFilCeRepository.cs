using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Tech
{
    public interface IConfFilCeRepository : IRepository<PhoenixConfContext, ConfFilCe, decimal>
    {
    }
}
