using System;

namespace VPBANK.RMD.Services.Collection.DataContracts.Imports
{
    public class ConCollectionListExcel
    {
        public DateTime Business_Date { get; set; }
        public string Type { get; set; }
        public DateTime Classify_BB_Date { get; set; }
        public string Contract_Id { get; set; }
        public string Customer_Id { get; set; }
        public string Os_Company { get; set; }
        public DateTime CL_OS_Start_Date { get; set; }
        public string Product_Group { get; set; }
        public string Reason { get; set; }
        public DateTime CL_OS_End_Date { get; set; }
        public string Note_Transfer { get; set; }
    }
}
