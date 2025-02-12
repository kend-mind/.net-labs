using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App
{
    public interface IApproveStatusRepository : IRepository<PhoenixConfContext, ApproveStatus, int>
    {
        IList<ApproveStatus> FindAllByFkRequestIdAndStatus(int reqId, string reqStatus);
    }
}
