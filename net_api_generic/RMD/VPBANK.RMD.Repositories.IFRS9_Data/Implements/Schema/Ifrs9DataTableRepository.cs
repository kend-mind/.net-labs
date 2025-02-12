using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using VPBANK.RMD.Data.IFRS9_Data;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.IFRS9_Data.Interfaces.Schema;

namespace VPBANK.RMD.Repositories.IFRS9_Data.Implements.Schema
{
    public class Ifrs9DataTableRepository : QueryRepository<IFRS9_DataContext, TableInfo>, IIfrs9DataTableRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly IFRS9_DataContext _context;

        public Ifrs9DataTableRepository(IDistributedCache distributedCache,
            IQueryableRepository<IFRS9_DataContext, TableInfo> queryableRepository,
            IFRS9_DataContext context) : base(queryableRepository)
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
