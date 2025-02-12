using System;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.PhoenixConf.Interfaces.App
{
    public interface IRequestObjectService
    {
        RequestObject InsertOrUpdate(RequestObject req, string reqStatus, RequestStep step, string username, DateTime currentDate);
    }
}
