using System.Collections.Generic;
using VPBANK.RMD.Data.IFRS9_Data;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Repositories.IFRS9_Data.Interfaces.Schema
{
    public interface IIfrs9DataColumnRepository : IQueryRepository<IFRS9_DataContext, ColumnInfo>
    {
        public IEnumerable<ColumnInfo> FindAll();
    }
}