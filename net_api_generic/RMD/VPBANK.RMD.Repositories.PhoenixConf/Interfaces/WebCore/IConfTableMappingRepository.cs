using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.WebCore;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.WebCore
{
    public interface IConfTableMappingRepository : IRepository<PhoenixConfContext, ConfTableMapping, int>
    {
    }
}
