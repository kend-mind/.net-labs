using System;
using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Services.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.Services.PhoenixConf.Implements.App
{
    public class ApproveStatusService : IApproveStatusService
    {
        private readonly IUnitOfWork<PhoenixConfContext> _unitOfWork;
        private readonly IApproveStatusRepository _repository;

        public ApproveStatusService(IUnitOfWork<PhoenixConfContext> unitOfWork, IApproveStatusRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IList<ApproveStatus> FindAllByFkRequestIdAndStatus(int reqId, string reqStatus)
        {
            return _repository.FindAllByFkRequestIdAndStatus(reqId, reqStatus);
        }

        public void Insert(int reqId, string status, RequestStep step, string commnet, string username)
        {
            var appStatus = new ApproveStatus
            {
                Pk_Id = 0,
                Fk_Request_Id = reqId,
                Approver = username,
                Status = status,
                Step = (int)step,
                Is_End = !RequestStatus.PENDING.Equals(status, StringComparison.CurrentCultureIgnoreCase),
                Comment = commnet,
                Created_Dt = DateTime.Now
            };
            _repository.Insert(appStatus);
        }
    }
}
