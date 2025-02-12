using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Schema
{
    public interface IConfTableRepository : IQueryRepository<PhoenixConfContext, TableInfo>
    {
        public IEnumerable<TableInfo> FindAll();
    }
}