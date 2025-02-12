using System.Collections.Generic;
using VPBANK.RMD.Data.IFRS9_Conf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Repositories.IFRS9_Conf.Interfaces.Schema
{
    public interface IIfrs9ConfTableRepository : IQueryRepository<IFRS9_ConfContext, TableInfo>
    {
        public IEnumerable<TableInfo> FindAll();
    }
}