using System.Collections.Generic;
using System.Data;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface IConCollectionListService
    {
        Dictionary<int, string> ValidateExcelFileImport(DataTable dataTable);
        Dictionary<int, string> ValidateDatFileImport(string content);
        void DataImport(List<DataTable> dataTables);
    }
}
