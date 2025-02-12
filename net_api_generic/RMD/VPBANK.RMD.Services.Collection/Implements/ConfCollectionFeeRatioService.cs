using System;
using System.Collections.Generic;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Datas;
using VPBANK.RMD.Utils.Common.Shared;
namespace VPBANK.RMD.Services.Collection.Implements
{
    public class ConfCollectionFeeRatioService : IConfCollectionFeeRatioService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, ConfCollectionFeeRatio, int> _genCollecFeeRatioRepository;
        public ConfCollectionFeeRatioService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, ConfCollectionFeeRatio, int> genCollecFeeRatioRepository)
        {
            _unitOfWork = unitOfWork;
            _genCollecFeeRatioRepository = genCollecFeeRatioRepository;
        }

        public IList<FieldValidateResponse> Validate(ConfCollectionFeeRatioDto entity)
        {
            var results = new List<FieldValidateResponse>();
            try
            {
                if (string.IsNullOrEmpty(entity.Os_Company))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Os_Company)),
                        Field = nameof(entity.Os_Company),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Os_Company))
                    });
                if (string.IsNullOrEmpty(entity.Dpd_Before_Pmt_Date))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Dpd_Before_Pmt_Date)),
                        Field = nameof(entity.Dpd_Before_Pmt_Date),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Dpd_Before_Pmt_Date))
                    });

                if (string.IsNullOrEmpty(entity.Sell_Loan))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Sell_Loan)),
                        Field = nameof(entity.Sell_Loan),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Sell_Loan))
                    });

                if (string.IsNullOrEmpty(entity.Dead_Loan))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Dead_Loan)),
                        Field = nameof(entity.Dead_Loan),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Dead_Loan))
                    });

                if (string.IsNullOrEmpty(entity.Product))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Product)),
                        Field = nameof(entity.Product),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Product))
                    });

                if (!entity.Fee_Ratio.HasValue)
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM001, nameof(entity.Fee_Ratio)),
                        Field = nameof(entity.Fee_Ratio),
                        Description = string.Format(ErrorMessages.EM001, nameof(entity.Fee_Ratio))
                    });

                if (!string.IsNullOrEmpty(entity.Sell_Loan) && entity.Sell_Loan.ToUpper() == Constants.NO && string.IsNullOrEmpty(entity.Dpd_Before_Cl_Os_Start_Date))
                    results.Add(new FieldValidateResponse
                    {
                        Error = ErrorMessages.EM146,
                        Field = nameof(entity.Dpd_Before_Cl_Os_Start_Date),
                        Description = ErrorMessages.EM146
                    });

                if (!string.IsNullOrEmpty(entity.Dead_Loan) && entity.Dead_Loan.ToUpper() == Constants.NO && string.IsNullOrEmpty(entity.Not_Bad_Debt_Flag))
                    results.Add(new FieldValidateResponse
                    {
                        Error = ErrorMessages.EM147,
                        Field = nameof(entity.Not_Bad_Debt_Flag),
                        Description = ErrorMessages.EM147
                    });

                if (entity.End_Date.HasValue && entity.End_Date.Value.Date < entity.Start_Date.Date)
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM019, "End Date", "Start Date"),
                        Field = nameof(entity.End_Date),
                        Description = string.Format(ErrorMessages.EM019, "End Date", "Start Date")
                    });
            }
            catch (Exception ex)
            {
                results.Add(new FieldValidateResponse
                {
                    Error = ex.Message,
                    Field = Constants.BUSSINESS_EXCEPTION,
                    Description = ex.Message
                });
            }
            return results;
        }
    }
}
