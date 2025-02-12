using System.Collections.Generic;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface ICollectionRepayAdjService
    {
        Dictionary<int, string> ValidateDatFileImport(string content);
    }
}
