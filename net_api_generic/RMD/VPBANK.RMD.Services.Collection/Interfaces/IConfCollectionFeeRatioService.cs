using System.Collections.Generic;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface IConfCollectionFeeRatioService
    {
        IList<FieldValidateResponse> Validate(ConfCollectionFeeRatioDto entity);
    }
}
