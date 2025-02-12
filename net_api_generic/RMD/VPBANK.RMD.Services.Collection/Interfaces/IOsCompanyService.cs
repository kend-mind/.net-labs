using System.Collections.Generic;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Entities.Commons;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface IOsCompanyService
    {
        IEnumerable<SelectedItem> GetGui(bool? hasAll);
        IList<FieldValidateResponse> Validate(OsCompanyDto entity);
    }
}
