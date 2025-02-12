using System;
using System.Threading.Tasks;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App
{
    public interface IEmailNotiRepository : IExecuteRepository<PhoenixConfContext>
    {
        Task SendEmailToOs(DateTime businessDate, string segment, string osCompany);
    }
}
