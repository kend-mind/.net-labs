using FluentFTP;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Repositories.Collection.Interfaces;
using VPBANK.RMD.Services.Collection.DataContracts.Imports;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Helpers;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class ConCollectionListService : IConCollectionListService
    {
        private readonly IConCollectionListRepository _repository;
        private readonly IGenericRepository<CollectionContext, ConCollectionListDaily, long> _genericRepository;

        public ConCollectionListService(IConCollectionListRepository repository,
            IGenericRepository<CollectionContext, ConCollectionListDaily, long> genericRepository)
        {
            _repository = repository;
            _genericRepository = genericRepository;
        }

        public Dictionary<int, string> ValidateExcelFileImport(DataTable dataTable)
        {
            //var errors = new StringBuilder();
            var errors = new Dictionary<int, string>();
            var rowIndex = 1;
            if (dataTable == null || dataTable.Rows.Count == 0)
                return errors;
            var actions = new List<string> { RecordStates.NEW, RecordStates.UPDATE };

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var rowErrors = new List<string>();
                    var conCollExcel = new ConCollectionListExcel();

                    var type = Convert.ToString(row[nameof(conCollExcel.Type)]);
                    var colClassifyBbDate = Convert.ToString(row[nameof(conCollExcel.Classify_BB_Date)]);
                    DateTime classifyBbDate;
                    var contractId = Convert.ToString(row[nameof(conCollExcel.Contract_Id)]);
                    var osCompany = Convert.ToString(row[nameof(conCollExcel.Os_Company)]);
                    var customerId = Convert.ToString(row[nameof(conCollExcel.Customer_Id)]);
                    var colClOsStartDate = Convert.ToString(row[nameof(conCollExcel.CL_OS_Start_Date)]);
                    DateTime clOsStartDate;
                    var productGroup = Convert.ToString(row[nameof(conCollExcel.Product_Group)]);
                    var reason = Convert.ToString(row[nameof(conCollExcel.Reason)]);
                    var colClOsEndDate = Convert.ToString(row[nameof(conCollExcel.CL_OS_End_Date)]);
                    DateTime clOsEndDate;

                    // Valid Data other col
                    if (!string.IsNullOrEmpty(type) && type.Equals(RecordStates.NEW, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Classify_BB_Date
                        if (string.IsNullOrEmpty(colClassifyBbDate))
                        {
                            classifyBbDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Classify_BB_Date)));
                        }
                        else
                        {
                            var isClassifyBbDate = DateTime.TryParseExact(colClassifyBbDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out classifyBbDate);
                            if (!isClassifyBbDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.Classify_BB_Date), DefFormats.DATE_FORMAT));
                        }

                        // CL_OS_Start_Date
                        if (string.IsNullOrEmpty(colClOsStartDate))
                        {
                            clOsStartDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.CL_OS_Start_Date)));
                        }
                        else
                        {
                            var isClOsStartDate = DateTime.TryParseExact(colClOsStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out clOsStartDate);
                            if (!isClOsStartDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.CL_OS_Start_Date), DefFormats.DATE_FORMAT));
                        }

                        // CL_OS_Start_Date >= Classify_BB_Date
                        if (classifyBbDate != null && classifyBbDate != DateTime.MinValue && clOsStartDate != null && clOsStartDate != DateTime.MinValue && DateTime.Compare(clOsStartDate, DateTime.Now) >= 0)
                            rowErrors.Add(ErrorMessages.EM123);
                        if (classifyBbDate != null && classifyBbDate != DateTime.MinValue && clOsStartDate != null && clOsStartDate != DateTime.MinValue && DateTime.Compare(clOsStartDate, classifyBbDate) <= 0)
                            rowErrors.Add(ErrorMessages.EM125);

                        // Customer_Id
                        if (string.IsNullOrEmpty(customerId))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Customer_Id)));

                        // Contract_Id
                        if (string.IsNullOrEmpty(contractId))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Contract_Id)));

                        // Contract_Id & CL_OS_Start_Date
                        if (!string.IsNullOrEmpty(contractId) && clOsStartDate != null && clOsStartDate != DateTime.MinValue && _repository.FindByContractIdAndClOsStartDate(contractId, clOsStartDate) != null)
                            rowErrors.Add(ErrorMessages.EM127);

                        // Os_Company
                        if (string.IsNullOrEmpty(osCompany))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Os_Company)));

                        // Product_Group
                        if (string.IsNullOrEmpty(productGroup))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Product_Group)));

                        // Other cases, must be NULL
                        if (!string.IsNullOrEmpty(reason))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Reason)));
                        if (!string.IsNullOrEmpty(colClOsEndDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.CL_OS_End_Date)));
                    }
                    else if (!string.IsNullOrEmpty(type) && type.Equals(RecordStates.UPDATE, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Reason
                        if (string.IsNullOrEmpty(reason))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Reason)));

                        // CL_OS_End_Date
                        if (row[nameof(conCollExcel.CL_OS_End_Date)] == null || string.IsNullOrEmpty(Convert.ToString(row[nameof(conCollExcel.CL_OS_End_Date)])))
                        {
                            clOsEndDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.CL_OS_End_Date)));
                        }
                        else
                        {
                            var isClOsEndDate = DateTime.TryParseExact(row[nameof(conCollExcel.CL_OS_End_Date)].ToString(), DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out clOsEndDate);
                            if (!isClOsEndDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.CL_OS_End_Date), DefFormats.DATE_FORMAT));
                        }

                        // Customer_Id & Contract_Id
                        if (string.IsNullOrEmpty(customerId) && string.IsNullOrEmpty(contractId))
                            rowErrors.Add(ErrorMessages.EM129);
                        else if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(contractId))
                            rowErrors.Add(ErrorMessages.EM130);
                        else
                        {
                            if (!string.IsNullOrEmpty(contractId))
                            {
                                var conCollByContractId = _repository.FindByContractId(contractId);
                                if (conCollByContractId == null)
                                    rowErrors.Add(ErrorMessages.EM131);
                                else if (!string.IsNullOrEmpty(conCollByContractId.Reason))
                                    rowErrors.Add(ErrorMessages.EM133);
                            }

                            if (!string.IsNullOrEmpty(customerId))
                            {
                                var conColls = _repository.FindAllByCustomerId(customerId);
                                if (conColls == null || conColls.Count() == 0)
                                    rowErrors.Add(ErrorMessages.EM132);
                                else if (conColls.Count(c => !string.IsNullOrEmpty(c.Reason)) > 0)
                                    rowErrors.Add(ErrorMessages.EM134);
                            }
                        }

                        // Other cases, must be NULL
                        if (!string.IsNullOrEmpty(colClassifyBbDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Classify_BB_Date)));
                        if (!string.IsNullOrEmpty(osCompany))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Os_Company)));
                        if (!string.IsNullOrEmpty(colClOsStartDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.CL_OS_Start_Date)));
                        if (!string.IsNullOrEmpty(productGroup))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Product_Group)));
                    }
                    else
                    {
                        rowErrors.Add(string.Format(ErrorMessages.EM072, nameof(conCollExcel.Type)));
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
                if (items == null || items.Length != 11) continue;

                try
                {
                    var rowErrors = new List<string>();
                    var conCollExcel = new ConCollectionListExcel();

                    var sBusinessDate = string.Empty;
                    if (!string.IsNullOrEmpty(items[0]))
                        sBusinessDate = Convert.ToString(items[0]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);

                    var type = Convert.ToString(items[1]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var colClassifyBbDate = Convert.ToString(items[2]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    DateTime classifyBbDate;
                    var contractId = Convert.ToString(items[3]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var customerId = Convert.ToString(items[4]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var osCompany = Convert.ToString(items[5]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var colClOsStartDate = Convert.ToString(items[6]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    DateTime clOsStartDate;
                    var productGroup = Convert.ToString(items[7]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var reason = Convert.ToString(items[8]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    var colClOsEndDate = Convert.ToString(items[9]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);
                    DateTime clOsEndDate;
                    var noteTransfer = Convert.ToString(items[10]).Replace(SpecificSystems.QUOTE_DOUBLE, string.Empty);

                    // Valid Data other col
                    if (!string.IsNullOrEmpty(type) && type.Equals(RecordStates.NEW, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Classify_BB_Date
                        if (string.IsNullOrEmpty(colClassifyBbDate))
                        {
                            classifyBbDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Classify_BB_Date)));
                        }
                        else
                        {
                            var isClassifyBbDate = DateTime.TryParseExact(colClassifyBbDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out classifyBbDate);
                            if (!isClassifyBbDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.Classify_BB_Date), DefFormats.DATE_FORMAT));
                        }

                        // CL_OS_Start_Date
                        if (string.IsNullOrEmpty(colClOsStartDate))
                        {
                            clOsStartDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.CL_OS_Start_Date)));
                        }
                        else
                        {
                            var isClOsStartDate = DateTime.TryParseExact(colClOsStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out clOsStartDate);
                            if (!isClOsStartDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.CL_OS_Start_Date), DefFormats.DATE_FORMAT));
                        }

                        // CL_OS_Start_Date >= Classify_BB_Date
                        if (classifyBbDate != null && classifyBbDate != DateTime.MinValue && clOsStartDate != null && clOsStartDate != DateTime.MinValue && DateTime.Compare(clOsStartDate, DateTime.Now) >= 0)
                            rowErrors.Add(ErrorMessages.EM123);
                        if (classifyBbDate != null && classifyBbDate != DateTime.MinValue && clOsStartDate != null && clOsStartDate != DateTime.MinValue && DateTime.Compare(clOsStartDate, classifyBbDate) <= 0)
                            rowErrors.Add(ErrorMessages.EM125);

                        // Customer_Id
                        if (string.IsNullOrEmpty(customerId))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Customer_Id)));

                        // Contract_Id
                        if (string.IsNullOrEmpty(contractId))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Contract_Id)));

                        //// Contract_Id & CL_OS_Start_Date
                        //if (!string.IsNullOrEmpty(contractId) && clOsStartDate != null && clOsStartDate != DateTime.MinValue && _repository.FindByContractIdAndClOsStartDate(contractId, clOsStartDate) != null)
                        //    rowErrors.Add(ErrorMessages.EM127);

                        // Os_Company
                        if (string.IsNullOrEmpty(osCompany))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Os_Company)));

                        // Product_Group
                        if (string.IsNullOrEmpty(productGroup))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Product_Group)));

                        // Note_Transfer
                        if (string.IsNullOrEmpty(noteTransfer))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Note_Transfer)));

                        // Other cases, must be NULL
                        if (!string.IsNullOrEmpty(reason))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Reason)));
                        if (!string.IsNullOrEmpty(colClOsEndDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.CL_OS_End_Date)));
                    }
                    else if (!string.IsNullOrEmpty(type) && type.Equals(RecordStates.UPDATE, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Reason
                        if (string.IsNullOrEmpty(reason))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Reason)));

                        // CL_OS_End_Date
                        if (string.IsNullOrEmpty(colClOsEndDate))
                        {
                            clOsEndDate = DateTime.MinValue;
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.CL_OS_End_Date)));
                        }
                        else
                        {
                            var isClOsEndDate = DateTime.TryParseExact(colClOsEndDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out clOsEndDate);
                            if (!isClOsEndDate)
                                rowErrors.Add(string.Format(ErrorMessages.EM002, nameof(conCollExcel.CL_OS_End_Date), DefFormats.DATE_FORMAT));
                        }

                        // Customer_Id & Contract_Id
                        if (string.IsNullOrEmpty(customerId) && string.IsNullOrEmpty(contractId))
                            rowErrors.Add(ErrorMessages.EM129);
                        else if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(contractId))
                            rowErrors.Add(ErrorMessages.EM130);
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(contractId))
                        //    {
                        //        var conCollByContractId = _repository.FindByContractId(contractId);
                        //        if (conCollByContractId == null)
                        //            rowErrors.Add(ErrorMessages.EM131);
                        //        else if (!string.IsNullOrEmpty(conCollByContractId.Reason))
                        //            rowErrors.Add(ErrorMessages.EM133);
                        //    }

                        //    if (!string.IsNullOrEmpty(customerId))
                        //    {
                        //        var conColls = _repository.FindAllByCustomerId(customerId);
                        //        if (conColls == null || conColls.Count() == 0)
                        //            rowErrors.Add(ErrorMessages.EM132);
                        //        else if (conColls.Count(c => !string.IsNullOrEmpty(c.Reason)) > 0)
                        //            rowErrors.Add(ErrorMessages.EM134);
                        //    }
                        //}

                        // Other cases, must be NULL
                        if (!string.IsNullOrEmpty(colClassifyBbDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Classify_BB_Date)));
                        if (!string.IsNullOrEmpty(osCompany))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Os_Company)));
                        if (!string.IsNullOrEmpty(colClOsStartDate))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.CL_OS_Start_Date)));
                        if (!string.IsNullOrEmpty(productGroup))
                            rowErrors.Add(string.Format(ErrorMessages.EM135, nameof(conCollExcel.Product_Group)));
                        if (!string.IsNullOrEmpty(noteTransfer))
                            rowErrors.Add(string.Format(ErrorMessages.EM001, nameof(conCollExcel.Note_Transfer)));
                    }
                    else
                    {
                        rowErrors.Add(string.Format(ErrorMessages.EM072, nameof(conCollExcel.Type)));
                    }

                    if (rowErrors.Count > 0)
                        errors.Add(i, rowErrors.Join(SpecificSystems.SEMICOLON));
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                    continue;
                }
                finally
                {
                }
            }

            return errors;
        }

        public void DataImport(List<DataTable> dataTables)
        {
            try
            {
                if (dataTables != null && dataTables.Count > 0)
                {
                    var conCollExcel = new ConCollectionListExcel();

                    var updConColls = new List<object>();
                    var newConColls = new List<object>();

                    foreach (var dataTable in dataTables)
                    {
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                var type = Convert.ToString(row[nameof(conCollExcel.Type)]);
                                if (string.IsNullOrEmpty(type)) continue;

                                // case insert
                                if (type.Equals(RecordStates.NEW, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var colBusinessDate = Convert.ToString(row[nameof(conCollExcel.Business_Date)]);
                                    var colClassifyBbDate = Convert.ToString(row[nameof(conCollExcel.Classify_BB_Date)]);
                                    var colClOsStartDate = Convert.ToString(row[nameof(conCollExcel.CL_OS_Start_Date)]);

                                    newConColls.Add(new ConCollectionListDaily
                                    {
                                        Pk_Id = 0,
                                        Business_Date = DateTime.ParseExact(colBusinessDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture),
                                        Classify_BB_Date = string.IsNullOrEmpty(colClassifyBbDate)
                                            ? (DateTime?)null
                                            : DateTime.ParseExact(colClassifyBbDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture),
                                        Contract_Id = Convert.ToString(row[nameof(conCollExcel.Contract_Id)]),
                                        Customer_Id = Convert.ToString(row[nameof(conCollExcel.Customer_Id)]),
                                        Os_Company = Convert.ToString(row[nameof(conCollExcel.Os_Company)]),
                                        CL_OS_Start_Date = DateTime.ParseExact(colClOsStartDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture),
                                        Product_Group = Convert.ToString(row[nameof(conCollExcel.Product_Group)]),
                                        Region = null,
                                        CL_OS_End_Date = null,
                                        CL_OS_End_Date_Fee = null,
                                        Reason = null,
                                        Closing_Month = null,
                                        Note_Transfer = Convert.ToString(row[nameof(conCollExcel.Note_Transfer)])
                                    });
                                }

                                // case update
                                if (type.Equals(RecordStates.UPDATE, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var customerId = Convert.ToString(row[nameof(conCollExcel.Customer_Id)]);
                                    var contractId = Convert.ToString(row[nameof(conCollExcel.Contract_Id)]);
                                    var reason = Convert.ToString(row[nameof(conCollExcel.Reason)]);
                                    var colClOsEndDate = Convert.ToString(row[nameof(conCollExcel.CL_OS_End_Date)]);
                                    var clOsEndDate = DateTime.ParseExact(colClOsEndDate, DefFormats.DATE_FORMAT, CultureInfo.InvariantCulture);

                                    if (!string.IsNullOrEmpty(customerId))
                                    {
                                        var conColls = _repository.FindAllByCustomerId(customerId);
                                        if (conColls != null && conColls.Count() > 0)
                                        {
                                            foreach (var conColl in conColls)
                                            {
                                                if (string.IsNullOrEmpty(conColl.Reason))
                                                {
                                                    conColl.Reason = reason;
                                                    conColl.CL_OS_End_Date = clOsEndDate;
                                                    if (reason.Equals(CollectionReason.PLN.ToDescription(), StringComparison.CurrentCultureIgnoreCase))
                                                    {
                                                        conColl.CL_OS_End_Date_Fee = clOsEndDate.AddDays(-1);
                                                        conColl.Closing_Month = clOsEndDate.AddDays(-1).ToString(DefFormats.DATE_YYYYMM_);
                                                    }
                                                    else
                                                    {
                                                        conColl.CL_OS_End_Date_Fee = clOsEndDate;
                                                        conColl.Closing_Month = clOsEndDate.ToString(DefFormats.DATE_YYYYMM_);
                                                    }

                                                    updConColls.Add(conColl);
                                                }
                                            }
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(contractId))
                                    {
                                        var conColl = _repository.FindByContractId(contractId);
                                        if (conColl != null && string.IsNullOrEmpty(conColl.Reason))
                                        {
                                            conColl.Reason = reason;
                                            conColl.CL_OS_End_Date = clOsEndDate;
                                            if (reason.Equals(CollectionReason.PLN.ToDescription(), StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                conColl.CL_OS_End_Date_Fee = clOsEndDate.AddDays(-1);
                                                conColl.Closing_Month = clOsEndDate.AddDays(-1).ToString(DefFormats.DATE_YYYYMM_);
                                            }
                                            else
                                            {
                                                conColl.CL_OS_End_Date_Fee = clOsEndDate;
                                                conColl.Closing_Month = clOsEndDate.ToString(DefFormats.DATE_YYYYMM_);
                                            }

                                            updConColls.Add(conColl);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _genericRepository.BulkUpdate(updConColls);
                    _genericRepository.BulkInsert(newConColls);
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
