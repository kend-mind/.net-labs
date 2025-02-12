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
    public class CollectionDpdManualService : ICollectionDpdManualService
    {
        private readonly IGenericRepository<CollectionContext, CollectionDpdManual, long> _genericRepository;

        public CollectionDpdManualService(IGenericRepository<CollectionContext, CollectionDpdManual, long> genericRepository)
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
                    var dpdManual = new CollectionDpdManual();

                    var sContractId = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sContractId = Convert.ToString(items[0]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sClOsStartDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sClOsStartDate = Convert.ToString(items[1]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sDpd = string.Empty;
                    if (!string.IsNullOrEmpty(items[2]))
                        sDpd = Convert.ToString(items[2]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    // Cl_Os_Start_Date
                    if (string.IsNullOrEmpty(sClOsStartDate))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dpdManual.Cl_Os_Start_Date)));
                    // Contract_ID
                    if (string.IsNullOrEmpty(sContractId))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dpdManual.Contract_Id)));
                    if (!string.IsNullOrEmpty(sContractId) && sContractId.Length > 50)
                        rowErrors.Add(string.Format(ErrorMessages.EM003, nameof(dpdManual.Contract_Id), 50));
                    // Dpd
                    if (string.IsNullOrEmpty(sDpd))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dpdManual.Dpd)));

                    var isClOsStartDate = DateTime.TryParseExact(sClOsStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime businessDate);
                    if (!isClOsStartDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dpdManual.Cl_Os_Start_Date), DefFormats.DATE_FORMAT));

                    var isDpd = int.TryParse(sDpd, NumberStyles.None, CultureInfo.InvariantCulture, out int dpd);
                    if (!isDpd)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dpdManual.Dpd), DefFormats.INT_FORMAT));

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
