using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Repositories.Collection.Interfaces.Schema
{
    public interface IColnTableRepository : IQueryRepository<CollectionContext, TableInfo>
    {
        public IEnumerable<TableInfo> FindAll();
    }
}