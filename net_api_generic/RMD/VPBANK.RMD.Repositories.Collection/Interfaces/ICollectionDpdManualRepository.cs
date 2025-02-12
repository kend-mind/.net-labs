using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Collection.Interfaces
{
    public interface ICollectionDpdManualRepository : IRepository<CollectionContext, CollectionDpdManual, long>
    {
        Task<int> DeleteAllAsync();
    }
}
