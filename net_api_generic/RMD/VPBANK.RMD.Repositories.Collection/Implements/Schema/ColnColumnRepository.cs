using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.Collection.Implements.Schema
{
    public class ColnColumnRepository : QueryRepository<CollectionContext, ColumnInfo>, IColnColumnRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CollectionContext _context;

        public ColnColumnRepository(IDistributedCache distributedCache,
            IQueryableRepository<CollectionContext, ColumnInfo> queryableRepository,
            CollectionContext context) : base(queryableRepository)
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
