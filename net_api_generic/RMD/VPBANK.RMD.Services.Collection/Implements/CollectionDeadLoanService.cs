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
    public class CollectionDeadLoanService : ICollectionDeadLoanService
    {
        private readonly IGenericRepository<CollectionContext, CollectionDeadLoan, long> _genericRepository;

        public CollectionDeadLoanService(IGenericRepository<CollectionContext, CollectionDeadLoan, long> genericRepository)
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
                    var dealLoan = new CollectionDeadLoan();

                    // CIF
                    if (row[nameof(dealLoan.Customer_Id)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(dealLoan.Customer_Id)])))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dealLoan.Customer_Id)));

                    // Start_Date
                    if (row[nameof(dealLoan.Start_Date)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(dealLoan.Start_Date)])))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dealLoan.Start_Date)));

                    var isStartDate = DateTime.TryParseExact(row[nameof(dealLoan.Start_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dealLoan.Start_Date), DefFormats.DATE_FORMAT));

                    // End_Date
                    if (row[nameof(dealLoan.End_Date)] != null && !string.IsNullOrEmpty(Convert.ToString(row[nameof(dealLoan.End_Date)])))
                    {
                        var isEndDate = DateTime.TryParseExact(row[nameof(dealLoan.End_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                        if (!isEndDate)
                            rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dealLoan.End_Date), DefFormats.DATE_FORMAT));
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
                    var dealLoan = new CollectionDeadLoan();

                    var sCif = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sCif = Convert.ToString(items[0]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sStartDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sStartDate = Convert.ToString(items[1]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var sEndDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[1]))
                        sEndDate = Convert.ToString(items[2]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);

                    // CIF
                    if (string.IsNullOrEmpty(sCif))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dealLoan.Customer_Id)));

                    // Start_Date
                    if (string.IsNullOrEmpty(sStartDate))
                        rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(dealLoan.Start_Date)));

                    var isStartDate = DateTime.TryParseExact(sStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDate)
                        rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dealLoan.Start_Date), DefFormats.DATE_FORMAT));

                    // End_Date
                    if (!string.IsNullOrEmpty(sEndDate))
                    {
                        var isEndDate = DateTime.TryParseExact(sEndDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                        if (!isEndDate)
                            rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(dealLoan.End_Date), DefFormats.DATE_FORMAT));
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
                    var dealLoans = new List<object>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dealLoan = new CollectionDeadLoan { Pk_Id = 0 };
                        dealLoan.Customer_Id = Convert.ToString(row[nameof(dealLoan.Customer_Id)]);
                        dealLoan.Start_Date = DateTime.ParseExact(row[nameof(dealLoan.Start_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture);
                        dealLoan.End_Date = row[nameof(dealLoan.End_Date)] == null || string.IsNullOrEmpty(row[nameof(dealLoan.End_Date)].ToString())
                            ? (DateTime?)null
                            : DateTime.ParseExact(row[nameof(dealLoan.End_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture);
                        dealLoans.Add(dealLoan);
                    }
                    _genericRepository.BulkInsert(dealLoans);
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
