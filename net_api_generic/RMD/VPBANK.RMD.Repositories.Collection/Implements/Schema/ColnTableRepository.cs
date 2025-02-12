using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.Collection.Implements.Schema
{
    public class ColnTableRepository : QueryRepository<CollectionContext, TableInfo>, IColnTableRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly CollectionContext _context;

        public ColnTableRepository(IDistributedCache distributedCache,
            IQueryableRepository<CollectionContext, TableInfo> queryableRepository,
            CollectionContext context) : base(queryableRepository)
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
