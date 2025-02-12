using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Functions;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Tech;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.Tech
{
    public class AvaiableDateRepository : QueryRepository<PhoenixConfContext, AvaiableDate>, IAvaiableDateRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<AvaiableDate> _logger;
        protected readonly PhoenixConfContext _context;

        public AvaiableDateRepository(IDistributedCache distributedCache,
            ILogger<AvaiableDate> logger,
            IQueryableRepository<PhoenixConfContext, AvaiableDate> queryableRepository,
            PhoenixConfContext context) : base(queryableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _context = context;
        }

        public IEnumerable<AvaiableDate> FindCollectionAvaiDatesBySegment(string segment)
        {
            return QueryableRepository.QueryableSql($"select * from tech.get_Coln_Avail_Date ('{segment}')");
        }
    }
}
