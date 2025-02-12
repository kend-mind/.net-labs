using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Repositories.PhoenixConf.Implements.App
{
    public class EmailNotiRepository : ExecuteRepository<PhoenixConfContext>, IEmailNotiRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly PhoenixConfContext _context;

        public EmailNotiRepository(IExecuteableRepository<PhoenixConfContext> executeableRepository, IDistributedCache distributedCache,
            PhoenixConfContext context) : base(executeableRepository)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task SendEmailToOs(DateTime businessDate, string segment, string osCompany)
        {
            throw new NotImplementedException();
        }
    }
}
