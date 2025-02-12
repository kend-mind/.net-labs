using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixData.Interfaces.Core;

namespace VPBANK.RMD.Repositories.PhoenixData.Implements.Core
{
    public class OcePredealDataRepository : Repository<PhoenixDataContext, OcePredealData, long>, IOcePredealDataRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<OcePredealData> _logger;
        protected readonly PhoenixDataContext _phoenixDataContext;

        public OcePredealDataRepository(IDistributedCache distributedCache, ILogger<OcePredealData> logger, ITrackableRepository<PhoenixDataContext, OcePredealData, long> trackableRepository,
            PhoenixDataContext phoenixDataContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _phoenixDataContext = phoenixDataContext;
        }

        public IList<OcePredealData> FindAllByCustomerIdAndBusinessDate(string customerId, DateTime businessDate)
        {
            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.OCE_Predeal_Data AS c WHERE c.Customer_Id = {customerId} AND c.Business_Date = '{businessDate}'")
                .AsEnumerable()
                .ToList();
        }
    }
}
