using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.IFRS9_Conf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.IFRS9_Conf.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.IFRS9_Conf.Implements.Schema
{
    public class Ifrs9ConfTableRepository : QueryRepository<IFRS9_ConfContext, TableInfo>, IIfrs9ConfTableRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly IFRS9_ConfContext _context;

        public Ifrs9ConfTableRepository(IDistributedCache distributedCache,
            IQueryableRepository<IFRS9_ConfContext, TableInfo> queryableRepository,
            IFRS9_ConfContext context) : base(queryableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public IEnumerable<TableInfo> FindAll()
        {
            return QueryableRepository.QueryableSql(SchemaSqlQuery.INFORMATION_SCHEMA_TABLES);
        }
    }
}
