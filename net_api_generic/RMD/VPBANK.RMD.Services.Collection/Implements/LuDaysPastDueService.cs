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
using VPBANK.RMD.Utils.Common.Helpers;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class LuDaysPastDueService : ILuDaysPastDueService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, LuDaysPastDue, int> _genLuDaysPastDueRepository;

        public LuDaysPastDueService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, LuDaysPastDue, int> genLuDaysPastDueRepository)
        {
            _unitOfWork = unitOfWork;
            _genLuDaysPastDueRepository = genLuDaysPastDueRepository;
        }

        public IEnumerable<SelectedItem> GetGui(bool? hasAll)
        {
            try
            {
                var results = new List<SelectedItem>();
                if (hasAll.Value && hasAll.Value)
                    results.Add(new SelectedItem
                    {
                        Key = DataSystems.Select_Item_All,
                        Val = DataSystems.Select_Item_All
                    });
                results.AddRange(_genLuDaysPastDueRepository
                    .Queryable()
                    .AsEnumerable()
                    .Select(c => new SelectedItem
                    {
                        Key = c.Days_Past_Due_Dpd,
                        Val = c.Days_Past_Due_Dpd
                    })
                    .OrderBy(c => c.Key)
                    .Distinct()
                    .ToList());

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<FieldValidateResponse> Validate(LuDaysPastDueDto entity)
        {
            var results = new List<FieldValidateResponse>();
            try
            {
                string patternFirst = "";
                string patternLast = "";
                #region validate pattern [(012;021)]
                var validFormat = true;
                if (!(entity.Days_Past_Due_Dpd.StartsWith(SpecificSystems.BRACKET_FIRST) || entity.Days_Past_Due_Dpd.StartsWith(SpecificSystems.PARENTHESES_FIRST)))
                    validFormat = false;
                if (!(entity.Days_Past_Due_Dpd.EndsWith(SpecificSystems.BRACKET_LAST) || entity.Days_Past_Due_Dpd.EndsWith(SpecificSystems.PARENTHESES_LAST)))
                    validFormat = false;

                if (validFormat)
                {
                    var strDays = entity.Days_Past_Due_Dpd.Remove(entity.Days_Past_Due_Dpd.Length - 1);
                    strDays = strDays[1..];
                    var arrDays = strDays.Split(SpecificSystems.SEMICOLON);
                    patternFirst = arrDays[0];
                    patternLast = arrDays[1];
                    if (arrDays.Length != 2)
                        validFormat = false;
                    else if (string.IsNullOrEmpty(arrDays[0]) && string.IsNullOrEmpty(arrDays[1]))
                        validFormat = false;
                    else
                    {
                        bool isNumeric1 = string.IsNullOrEmpty(arrDays[0]) || int.TryParse(arrDays[0], out int n1);
                        bool isNumeric2 = string.IsNullOrEmpty(arrDays[1]) || int.TryParse(arrDays[1], out int n2);
                        if (!isNumeric1 || !isNumeric2)
                            validFormat = false;
                    }
                }

                if (!validFormat)
                {
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM002, nameof(entity.Days_Past_Due_Dpd), "[1234;98)"),
                        Field = nameof(entity.Days_Past_Due_Dpd),
                        Description = string.Format(ErrorMessages.EM002, nameof(entity.Days_Past_Due_Dpd), "[1234;98)")
                    });
                }

                #endregion

                var dayPastDueDpd = $"{entity.Days_Past_Due_Dpd.Substring(0, 1)}{(string.IsNullOrEmpty(patternFirst) ? "" : int.Parse(patternFirst).ToString())};{(string.IsNullOrEmpty(patternLast) ? "" : int.Parse(patternLast).ToString())}{entity.Days_Past_Due_Dpd.Substring(entity.Days_Past_Due_Dpd.Length - 1, 1)}";
                if (!entity.Pk_Id.HasValue || entity.Pk_Id == 0 || (!string.IsNullOrEmpty(entity.ReqActionType) && entity.ReqActionType.Equals(ActionTypes.INSERT.ToDescription(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    //Valid case insert
                    if (_genLuDaysPastDueRepository.Queryable().AsEnumerable().FirstOrDefault(c => dayPastDueDpd.Equals(c.Days_Past_Due_Dpd, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, entity.Days_Past_Due_Dpd),
                            Field = nameof(entity.Days_Past_Due_Dpd),
                            Description = string.Format(ErrorMessages.EM004, entity.Days_Past_Due_Dpd)
                        });
                    }
                }
                else
                {
                    //valid case update
                    if (_genLuDaysPastDueRepository.Queryable().AsEnumerable().FirstOrDefault(c => dayPastDueDpd.Equals(c.Days_Past_Due_Dpd, StringComparison.CurrentCultureIgnoreCase) && c.Pk_Id != entity.Pk_Id) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM004, nameof(entity.Days_Past_Due_Dpd)),
                            Field = nameof(entity.Days_Past_Due_Dpd),
                            Description = string.Format(ErrorMessages.EM004, nameof(entity.Days_Past_Due_Dpd))
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
