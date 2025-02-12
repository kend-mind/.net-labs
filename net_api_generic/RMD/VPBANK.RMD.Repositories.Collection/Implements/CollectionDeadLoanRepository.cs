using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class CollectionDeadLoanRepository : Repository<CollectionContext, CollectionDeadLoan, long>, ICollectionDeadLoanRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly CollectionContext _context;

        public CollectionDeadLoanRepository(IDistributedCache distributedCache, 
            ITrackableRepository<CollectionContext, CollectionDeadLoan, long> trackableRepository,
            CollectionContext phoenixDataContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _context = phoenixDataContext;
        }

        public async Task<int> DeleteAllAsync()
        {
            try
            {
                return await _context.Database.ExecuteSqlCommandAsync(CollectionSqlQuery.Collection_Dead_Loan_Truncate_Table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
