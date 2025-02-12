using System;
using System.Collections.Generic;
using System.Text;
using VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core;

namespace VPBANK.RMD.Services.PhoenixData.Interfaces.Core
{
    public interface IOcePredealDataService
    {
        IList<OcePredealData> FindAllByCustomerIdAndBusinessDate(string customerId, DateTime businessDate);
    }
}
