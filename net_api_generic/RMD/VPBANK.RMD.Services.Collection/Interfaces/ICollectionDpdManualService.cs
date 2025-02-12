using System.Collections.Generic;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface ICollectionDpdManualService
    {
        Dictionary<int, string> ValidateDatFileImport(string content);
    }
}
