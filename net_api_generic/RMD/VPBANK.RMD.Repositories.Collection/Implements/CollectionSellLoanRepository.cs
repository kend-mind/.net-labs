using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class CollectionSellLoanRepository : Repository<CollectionContext, CollectionSellLoan, long>, ICollectionSellLoanRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly CollectionContext _context;

        public CollectionSellLoanRepository(IDistributedCache distributedCache, 
            ILogger<CollectionSellLoan> logger, 
            ITrackableRepository<CollectionContext, CollectionSellLoan, long> trackableRepository,
            CollectionContext context) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<int> DeleteAllAsync()
        {
            try
            {
                return await _context.Database.ExecuteSqlCommandAsync(CollectionSqlQuery.Collection_Sell_Loan_Truncate_Table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
