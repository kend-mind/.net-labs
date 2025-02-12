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
    public class LuCampaignService : ILuCampaignService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, LuCampaign, int> _genLuCampaignRepository;
        public LuCampaignService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, LuCampaign, int> genLuCampaignRepository)
        {
            _unitOfWork = unitOfWork;
            _genLuCampaignRepository = genLuCampaignRepository;
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
                results.AddRange(_genLuCampaignRepository
                    .Queryable()
                    .AsEnumerable()
                    .Select(c => new SelectedItem
                    {
                        Key = c.Campaign_Code,
                        Val = c.Campaign_Code
                    })
                    .OrderBy(c => c.Key)
                    .Distinct()
                    .ToList());

                return results;
            }
            catch (Exception)
            {
                
            }
            return results;
        }

        public IList<FieldValidateResponse> Validate(LuCampaignDto entity)
        {
            var results = new List<FieldValidateResponse>();
            try
            {
                if (!entity.Pk_Id.HasValue || entity.Pk_Id == 0)
                {
                    //Valid case insert
                    if (_genLuCampaignRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Campaign_Code.Equals(c.Campaign_Code, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, entity.Campaign_Code),
                            Field = nameof(entity.Campaign_Code),
                            Description = string.Format(ErrorMessages.EM004, entity.Campaign_Code)
                        });
                    }
                }
                else
                {
                    //valid case update
                    if (_genLuCampaignRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Campaign_Code.Equals(c.Campaign_Code, StringComparison.CurrentCultureIgnoreCase) && c.Pk_Id != entity.Pk_Id) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, nameof(entity.Campaign_Code)),
                            Field = nameof(entity.Campaign_Code),
                            Description = string.Format(ErrorMessages.EM004, nameof(entity.Campaign_Code))
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
