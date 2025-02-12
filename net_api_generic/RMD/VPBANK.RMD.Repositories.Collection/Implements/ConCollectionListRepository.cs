using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class ConCollectionListRepository : Repository<CollectionContext, ConCollectionListDaily, long>, IConCollectionListRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly CollectionContext _context;

        public ConCollectionListRepository(IDistributedCache distributedCache,
            ITrackableRepository<CollectionContext, ConCollectionListDaily, long> trackableRepository,
            CollectionContext context) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public IEnumerable<ConCollectionListDaily> FindAllByCustomerId(string customerId)
        {
            //return TrackableRepository
            //    .Queryable()
            //    .AsEnumerable()
            //    .Where(c => !string.IsNullOrEmpty(c.Customer_Id) && c.Customer_Id.Equals(customerId, StringComparison.CurrentCultureIgnoreCase));

            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.Con_Collection_List AS c WHERE c.Customer_Id = '{customerId}'")
                .AsEnumerable();
        }

        public ConCollectionListDaily FindByContractId(string contractId)
        {
            //return TrackableRepository
            //    .Queryable()
            //    .AsEnumerable()
            //    .Where(c => !string.IsNullOrEmpty(c.Contract_Id) && c.Contract_Id.Equals(contractId, StringComparison.CurrentCultureIgnoreCase))
            //    .SingleOrDefault();

            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.Con_Collection_List AS c WHERE c.Contract_Id = '{contractId}'")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public ConCollectionListDaily FindByContractIdAndOsCompany(string contractId, string osCompany)
        {
            //return TrackableRepository
            //    .Queryable()
            //    .AsEnumerable()
            //    .Where(c => !string.IsNullOrEmpty(c.Contract_Id) && c.Contract_Id.Equals(contractId, StringComparison.CurrentCultureIgnoreCase) &&
            //                !string.IsNullOrEmpty(c.Os_Company) && c.Os_Company.Equals(osCompany, StringComparison.CurrentCultureIgnoreCase))
            //    .SingleOrDefault();

            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.Con_Collection_List AS c WHERE c.Contract_Id = '{contractId}' AND c.Os_Company = '{osCompany}'")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public ConCollectionListDaily FindByContractIdAndCustomerId(string contractId, string customerId)
        {
            //return TrackableRepository
            //    .Queryable()
            //    .AsEnumerable()
            //    .Where(c => !string.IsNullOrEmpty(c.Contract_Id) && c.Contract_Id.Equals(contractId, StringComparison.CurrentCultureIgnoreCase) &&
            //                !string.IsNullOrEmpty(c.Os_Company) && c.Os_Company.Equals(customerId, StringComparison.CurrentCultureIgnoreCase))
            //    .SingleOrDefault();

            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.Con_Collection_List AS c WHERE c.Contract_Id = '{contractId}' AND c.Customer_Id = '{customerId}'")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public ConCollectionListDaily FindByContractIdAndClOsStartDate(string contractId, DateTime clOsStartDate)
        {
            //return TrackableRepository
            //    .Queryable()
            //    .AsEnumerable()
            //    .Where(c => !string.IsNullOrEmpty(c.Contract_Id) && c.Contract_Id.Equals(contractId, StringComparison.CurrentCultureIgnoreCase) &&
            //                c.CL_OS_Start_Date != null && clOsStartDate != null && c.CL_OS_Start_Date.ToString(DefFormats.DATE_FORMAT).Equals(clOsStartDate.ToString(DefFormats.DATE_FORMAT)))
            //    .SingleOrDefault();

            return TrackableRepository
                .QueryableSql($"SELECT c.* FROM Core.Con_Collection_List AS c WHERE c.Contract_Id = '{contractId}' AND c.CL_OS_Start_Date = '{clOsStartDate.ToString(DefFormats.DATE_FORMAT)}'")
                .AsEnumerable()
                .FirstOrDefault();
        }
    }
}
