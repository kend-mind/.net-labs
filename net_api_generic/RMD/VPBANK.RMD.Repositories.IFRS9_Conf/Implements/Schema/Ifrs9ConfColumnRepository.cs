using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.IFRS9_Conf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.IFRS9_Conf.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.IFRS9_Conf.Implements.Schema
{
    public class Ifrs9ConfColumnRepository : QueryRepository<IFRS9_ConfContext, ColumnInfo>, IIfrs9ConfColumnRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IFRS9_ConfContext _context;

        public Ifrs9ConfColumnRepository(IDistributedCache distributedCache,
            IQueryableRepository<IFRS9_ConfContext, ColumnInfo> queryableRepository,
            IFRS9_ConfContext context) : base(queryableRepository)
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
