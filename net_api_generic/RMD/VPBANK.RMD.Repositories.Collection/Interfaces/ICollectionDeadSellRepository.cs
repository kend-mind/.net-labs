using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Collection.Interfaces
{
    public interface ICollectionSellLoanRepository : IRepository<CollectionContext, CollectionSellLoan, long>
    {
        Task<int> DeleteAllAsync();
    }
}
