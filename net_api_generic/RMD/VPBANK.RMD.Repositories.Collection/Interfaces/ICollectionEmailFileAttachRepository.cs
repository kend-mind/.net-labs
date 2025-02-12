using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Collection.Interfaces
{
    public interface ICollectionEmailFileAttachRepository : IRepository<CollectionContext, CollectionEmailFileAttach, int>
    {
        public List<CollectionEmailFileAttach> FindAllByFkCollecEmailId(int fkCollecEmailId);
    }
}