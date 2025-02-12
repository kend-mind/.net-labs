using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App
{
    public interface IFTPDocumentTypeRepository : IRepository<PhoenixConfContext, FTPDocumentType, int>
    {
    }
}
