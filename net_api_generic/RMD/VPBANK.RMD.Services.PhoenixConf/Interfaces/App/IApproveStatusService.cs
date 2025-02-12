using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.PhoenixConf.Interfaces.App
{
    public interface IApproveStatusService
    {
        IList<ApproveStatus> FindAllByFkRequestIdAndStatus(int reqId, string reqStatus);
        void Insert(int reqId, string status, RequestStep step, string commnet, string username);
    }
}
