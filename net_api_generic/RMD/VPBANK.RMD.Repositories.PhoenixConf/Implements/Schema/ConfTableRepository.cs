using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.Schema
{
    public class ConfTableRepository : QueryRepository<PhoenixConfContext, TableInfo>, IConfTableRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly PhoenixConfContext _context;

        public ConfTableRepository(IDistributedCache distributedCache,
            IQueryableRepository<PhoenixConfContext, TableInfo> queryableRepository,
            PhoenixConfContext context) : base(queryableRepository)
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
