using FluentFTP;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class CollectionSellLoanService : ICollectionSellLoanService
    {
        private readonly IGenericRepository<CollectionContext, CollectionSellLoan, long> _genericRepository;

        public CollectionSellLoanService(IGenericRepository<CollectionContext, CollectionSellLoan, long> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Dictionary<int, string> ValidateExcelFileImport(DataTable dataTable)
        {
            //var errors = new StringBuilder();
            var errors = new Dictionary<int, string>();
            var rowIndex = 1;
            if (dataTable == null || dataTable.Rows.Count == 0)
                return errors;

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var rowErrors = new List<string>();
                    var sellLoan = new CollectionSellLoan();

                    if (row[nameof(sellLoan.Contract_Id)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(sellLoan.Contract_Id)])))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Contract_Id)));

                    if (row[nameof(sellLoan.Os_Company)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(sellLoan.Os_Company)])))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Os_Company)));

                    // Start_Date
                    if (row[nameof(sellLoan.Start_Date)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(sellLoan.Start_Date)])))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Start_Date)));

                    var isStartDate = DateTime.TryParseExact(row[nameof(sellLoan.Start_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(sellLoan.Start_Date), DefFormats.DATE_FORMAT));

                    // End_Date
                    if (row[nameof(sellLoan.End_Date)] != null && !string.IsNullOrEmpty(Convert.ToString(row[nameof(sellLoan.End_Date)])))
                    {
                        var isEndDate = DateTime.TryParseExact(row[nameof(sellLoan.End_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                        if (!isEndDate)
                            rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(sellLoan.End_Date), DefFormats.DATE_FORMAT));
                    }

                    if (rowErrors.Count > 0)
                        errors.Add(rowIndex, rowErrors.Join(SpecificSystems.SEMICOLON));
                }
                catch (Exception)
                {
                    continue;
                }
                finally
                {
                    rowIndex++;
                }
            }

            return errors;
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
                    var sellLoan = new CollectionSellLoan();

                    var sContractId = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sContractId = Convert.ToString(items[0]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sOsCompany = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sOsCompany = Convert.ToString(items[1]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sStartDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sStartDate = Convert.ToString(items[2]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sEndDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sEndDate = Convert.ToString(items[3]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);

                    if (string.IsNullOrEmpty(sContractId))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Contract_Id)));

                    if (string.IsNullOrEmpty(sOsCompany))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Os_Company)));

                    // Start_Date
                    if (string.IsNullOrEmpty(sStartDate))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(sellLoan.Start_Date)));

                    var isStartDate = DateTime.TryParseExact(sStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(sellLoan.Start_Date), DefFormats.DATE_FORMAT));

                    // End_Date
                    if (!string.IsNullOrEmpty(Convert.ToString(sEndDate)))
                    {
                        var isEndDate = DateTime.TryParseExact(sEndDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                        if (!isEndDate)
                            rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(sellLoan.End_Date), DefFormats.DATE_FORMAT));
                    }

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

        public void DataImport(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var sellLoans = new List<object>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var sellLoan = new CollectionSellLoan { Pk_Id = 0 };
                        sellLoan.Contract_Id = Convert.ToString(row[nameof(sellLoan.Contract_Id)]);
                        sellLoan.Os_Company = Convert.ToString(row[nameof(sellLoan.Os_Company)]);
                        sellLoan.Start_Date = DateTime.ParseExact(row[nameof(sellLoan.Start_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture);
                        sellLoan.End_Date = row[nameof(sellLoan.End_Date)] == null || string.IsNullOrEmpty(row[nameof(sellLoan.End_Date)].ToString())
                            ? (DateTime?)null
                            : DateTime.ParseExact(row[nameof(sellLoan.End_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture);
                        sellLoans.Add(sellLoan);
                    }
                    _genericRepository.BulkInsert(sellLoans);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
