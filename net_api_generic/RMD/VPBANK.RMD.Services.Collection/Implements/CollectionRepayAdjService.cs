using FluentFTP;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class CollectionRepayAdjService : ICollectionRepayAdjService
    {
        private readonly IGenericRepository<CollectionContext, CollectionRepayAdj, long> _genericRepository;

        public CollectionRepayAdjService(IGenericRepository<CollectionContext, CollectionRepayAdj, long> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Dictionary<int, string> ValidateDatFileImport(string content)
        {
            //var errors = new StringBuilder();
            var errors = new Dictionary<int, string>();
            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                return errors;

            var datas = content.Split(SpecificSystems.NEXTLINE);
            var fileName = datas[0];
            var size = datas[1];
            var time = datas[2];
            var headers = datas[3].Split(SpecificSystems.PILE);
            Log.Information($"fileName: {fileName}");
            Log.Information($"size: {size}");
            Log.Information($"time: {time}");
            Log.Information($"headers: {JsonConvert.SerializeObject(headers, Formatting.Indented)}");

            // validate data
            for (int i = DAT_TAG_FILE_EX.COLN_START_LINE_VALIDATE; i < datas.Length; i++)
            {
                var items = datas[i].Split(SpecificSystems.PILE);

                try
                {
                    var rowErrors = new List<string>();
                    var repayAdj = new CollectionRepayAdj();

                    var sBusinessDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sBusinessDate = Convert.ToString(items[0]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sContractId = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sContractId = Convert.ToString(items[1]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sRepayAdjAmt = string.Empty;
                    if (!string.IsNullOrEmpty(items[2]))
                        sRepayAdjAmt = Convert.ToString(items[2]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    // Business_Date
                    if (string.IsNullOrEmpty(sBusinessDate))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(repayAdj.Business_Date)));
                    // Contract_ID
                    if (string.IsNullOrEmpty(sContractId))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(repayAdj.Contract_Id)));
                    if (!string.IsNullOrEmpty(sContractId) && sContractId.Length > 50)
                        rowErrors.Add(string.Format(ErrorMessages.EM003, nameof(repayAdj.Contract_Id),50));
                    // Repay_Adj_Amt
                    if (string.IsNullOrEmpty(sRepayAdjAmt))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(repayAdj.Repay_Adj_Amt)));

                    var isBusinessDate = DateTime.TryParseExact(sBusinessDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime businessDate);
                    if (!isBusinessDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(repayAdj.Business_Date), DefFormats.DATE_FORMAT));

                    // Repay_Adj_Amt
                    var isRepayAdjAmt = decimal.TryParse(sRepayAdjAmt, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal repayAdjAmt);
                    if (!isRepayAdjAmt)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(repayAdj.Repay_Adj_Amt), DefFormats.DECIMAL_FORMAT));

                    if (rowErrors.Count > 0)
                        errors.Add(i, rowErrors.Join(SpecificSystems.SEMICOLON));
                }
                catch (Exception)
                {
                    continue;
                }
                finally
                {
                }
            }

            return errors;
        }
    }
}
