using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixData.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.PhoenixData.Implements.Schema
{
    public class DataColumnRepository : QueryRepository<PhoenixDataContext, ColumnInfo>, IDataColumnRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly PhoenixDataContext _context;

        public DataColumnRepository(IDistributedCache distributedCache,
            IQueryableRepository<PhoenixDataContext, ColumnInfo> queryableRepository,
            PhoenixDataContext context) : base(queryableRepository)
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
