using System.Threading.Tasks;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech;

namespace VPBANK.RMD.Services.PhoenixConf.Interfaces.Tech
{
    public interface IConfFilCeService
    {
        Task<ConfFilCe> InsertAsync(ConfFilCe confFilCe);
    }
}
