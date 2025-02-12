using Serilog;
using System;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Services.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.PhoenixConf.Implements.App
{
    public class RequestObjectService : IRequestObjectService
    {
        private readonly IUnitOfWork<PhoenixConfContext> _unitOfWork;
        private readonly IRequestObjectRepository _requestRepository;
        private readonly IApproveStatusRepository _reqStatusRepository;

        public RequestObjectService(IUnitOfWork<PhoenixConfContext> unitOfWork, IRequestObjectRepository requestRepository, IApproveStatusRepository reqStatusRepository)
        {
            _unitOfWork = unitOfWork;
            _requestRepository = requestRepository;
            _reqStatusRepository = reqStatusRepository;
        }

        public RequestObject InsertOrUpdate(RequestObject req, string reqStatus, RequestStep step, string username, DateTime currentDate)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // req object
                if (req.Pk_Id == 0)
                    _requestRepository.Insert(req);
                else
                    _requestRepository.Update(req);
                _unitOfWork.SaveChanges();

                // app status
                _reqStatusRepository.Insert(new ApproveStatus
                {
                    Fk_Request_Id = req.Pk_Id,
                    Status = reqStatus,
                    Step = (int)step,
                    Approver = username,
                    Comment = req.Comment,
                    Is_End = !(reqStatus.Equals(RequestStatus.PENDING, StringComparison.CurrentCultureIgnoreCase) || reqStatus.Equals(RequestStatus.CANCEL, StringComparison.CurrentCultureIgnoreCase)),
                    Created_Dt = currentDate
                });
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException.Message);
                throw ex;
            }
            finally
            {
                _unitOfWork.Commit();
            }
            return req;
        }
    }
}
