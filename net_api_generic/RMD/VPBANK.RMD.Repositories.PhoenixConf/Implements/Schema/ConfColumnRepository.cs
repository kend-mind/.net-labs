using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.Schema
{
    public class ConfColumnRepository : QueryRepository<PhoenixConfContext, ColumnInfo>, IConfColumnRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly PhoenixConfContext _context;

        public ConfColumnRepository(IDistributedCache distributedCache,
            IQueryableRepository<PhoenixConfContext, ColumnInfo> queryableRepository,
            PhoenixConfContext context) : base(queryableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public IEnumerable<ColumnInfo> FindAll()
        {
            return QueryableRepository.QueryableSql(SchemaSqlQuery.INFORMATION_SCHEMA_COLUMNS);
        }
    }
}
