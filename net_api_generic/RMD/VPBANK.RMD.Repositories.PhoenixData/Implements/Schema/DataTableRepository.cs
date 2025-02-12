using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixData.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.PhoenixData.Implements.Schema
{
    public class DataTableRepository : QueryRepository<PhoenixDataContext, TableInfo>, IDataTableRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly PhoenixDataContext _context;

        public DataTableRepository(IDistributedCache distributedCache,
            IQueryableRepository<PhoenixDataContext, TableInfo> queryableRepository,
            PhoenixDataContext context) : base(queryableRepository)
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
