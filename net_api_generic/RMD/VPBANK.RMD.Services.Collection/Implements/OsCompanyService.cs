using System;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Entities.Commons;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Datas;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class OsCompanyService : IOsCompanyService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, OsCompany, int> _genOsCompanyRepository;

        public OsCompanyService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, OsCompany, int> genOsCompanyRepository)
        {
            _unitOfWork = unitOfWork;
            _genOsCompanyRepository = genOsCompanyRepository;
        }

        public IEnumerable<SelectedItem> GetGui(bool? hasAll)
        {
            var results = new List<SelectedItem>();
            try
            {
                if (hasAll.Value && hasAll.Value)
                    results.Add(new SelectedItem
                    {
                        Key = DataSystems.Select_Item_All,
                        Val = DataSystems.Select_Item_All
                    });
                results.AddRange(_genOsCompanyRepository
                    .Queryable()
                    .AsEnumerable()
                    .Select(c => new SelectedItem
                    {
                        Key = c.Os_Company,
                        Val = c.Os_Company
                    })
                    .OrderBy(c => c.Key)
                    .Distinct()
                    .ToList());
            }
            catch (Exception)
            {
                
            }
            return results;
        }

        public IList<FieldValidateResponse> Validate(OsCompanyDto entity)
        {
            var results = new List<FieldValidateResponse>();
            try
            {
                if (!entity.Pk_Id.HasValue || entity.Pk_Id == 0)
                {
                    //Valid case insert
                    if (_genOsCompanyRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Os_Company.Equals(c.Os_Company, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, entity.Os_Company),
                            Field = nameof(entity.Os_Company),
                            Description = string.Format(ErrorMessages.EM004, entity.Os_Company)
                        });
                    }
                }
                else
                {
                    //valid case update
                    if (_genOsCompanyRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Os_Company.Equals(c.Os_Company, StringComparison.CurrentCultureIgnoreCase) && c.Pk_Id != entity.Pk_Id) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, nameof(entity.Os_Company)),
                            Field = nameof(entity.Os_Company),
                            Description = string.Format(ErrorMessages.EM004, nameof(entity.Os_Company))
                        });
                    }
                }
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
