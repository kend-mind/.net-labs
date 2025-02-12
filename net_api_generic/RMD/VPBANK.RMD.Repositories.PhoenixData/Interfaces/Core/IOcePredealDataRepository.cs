using System;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixData.Interfaces.Core
{
    public interface IOcePredealDataRepository : IRepository<PhoenixDataContext, OcePredealData, long>
    {
        IList<OcePredealData> FindAllByCustomerIdAndBusinessDate(string customerId, DateTime businessDate);
    }
}
