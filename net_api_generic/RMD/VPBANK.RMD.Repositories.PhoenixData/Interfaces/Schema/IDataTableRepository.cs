using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixData;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Repositories.PhoenixData.Interfaces.Schema
{
    public interface IDataTableRepository : IQueryRepository<PhoenixDataContext, TableInfo>
    {
        public IEnumerable<TableInfo> FindAll();
    }
}