using System;
using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Collection.Interfaces
{
    public interface IConCollectionListRepository : IRepository<CollectionContext, ConCollectionListDaily, long>
    {
        IEnumerable<ConCollectionListDaily> FindAllByCustomerId(string customerId);
        ConCollectionListDaily FindByContractId(string contractId);
        ConCollectionListDaily FindByContractIdAndOsCompany(string contractId, string osCompany);
        ConCollectionListDaily FindByContractIdAndCustomerId(string contractId, string customerId);
        ConCollectionListDaily FindByContractIdAndClOsStartDate(string contractId, DateTime clOsStartDate);
    } 
}
