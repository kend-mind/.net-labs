using System.Collections.Generic;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection.Views;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface ICollectionOutsourceEmailService
    {
        Task InsertAsync(ViewCollectionEmailDto entity);
        Task InsertListAsync(List<ViewCollectionEmailDto> entities);
        Task UpdateAsync(ViewCollectionEmailDto entity);
        Task UpdateListAsync(List<ViewCollectionEmailDto> entities);
        Task DeleteAsync(int pk_Id);
        Task DeleteListAsync(List<int> pk_Ids);
        IList<FieldValidateResponse> Validate(ViewCollectionEmailDto entity);
    }
}