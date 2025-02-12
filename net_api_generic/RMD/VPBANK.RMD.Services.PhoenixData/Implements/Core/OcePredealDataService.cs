using System;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Repositories.PhoenixData.Interfaces.Core;
using VPBANK.RMD.Services.PhoenixData.Interfaces.Core;

namespace VPBANK.RMD.Services.PhoenixData.Implements.Core
{
    public class OcePredealDataService : IOcePredealDataService
    {
        private readonly IGenericRepository<PhoenixDataContext, OcePredealData, long> _genericRepository;
        private readonly IOcePredealDataRepository _ocePredealDataRepository;

        public OcePredealDataService(IGenericRepository<PhoenixDataContext, OcePredealData, long> genericRepository,
            IOcePredealDataRepository ocePredealDataRepository)
        {
            _genericRepository = genericRepository;
            _ocePredealDataRepository = ocePredealDataRepository;
        }

        public IList<OcePredealData> FindAllByCustomerIdAndBusinessDate(string customerId, DateTime businessDate)
        {
            return _ocePredealDataRepository.FindAllByCustomerIdAndBusinessDate(customerId, businessDate);
        }
    }
}
