using System;
using System.Threading.Tasks;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Tech;
using VPBANK.RMD.Services.PhoenixConf.Interfaces.Tech;

namespace VPBANK.RMD.Services.PhoenixConf.Implements.Tech
{
    public class ConfFilCeService : IConfFilCeService
    {
        private readonly IUnitOfWork<PhoenixConfContext> _unitOfWork;
        private readonly IConfFilCeRepository _repository;

        public ConfFilCeService(IUnitOfWork<PhoenixConfContext> unitOfWork, IConfFilCeRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ConfFilCe> InsertAsync(ConfFilCe confFilCe)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _repository.Insert(confFilCe);
                int success = await _unitOfWork.SaveChangesAsync();

                if (success == 0)
                {
                    _unitOfWork.Rollback();
                    return null;
                }

                _unitOfWork.Commit();
                return confFilCe;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
