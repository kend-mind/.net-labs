using System;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.StoredProcedures
{
    public class CollectionRepayReport : BaseQueryResult
    {
        public DateTime? Business_Date { get; set; }
        public DateTime? Ngay_Chuyen_Bb { get; set; }
        public string Cif { get; set; }
        public string Contract_No { get; set; }
        public decimal? Pr_Repay { get; set; }
        public decimal? In_Repay { get; set; }
        public decimal? Pd_Repay { get; set; }
        public decimal? Ld_Repay { get; set; }
        public decimal? Total_Repay { get; set; }
        public string Os_Company { get; set; }
        public DateTime? Ngay_Pl_Os { get; set; }
        public DateTime? Ngay_Ket_Thuc { get; set; }
        public string Product_Group { get; set; }
        public string Region { get; set; }
        public string Business_Month { get; set; }
        public string Ban_No_Glx { get; set; }
        public decimal? Current_Total_Exposure { get; set; }
        public decimal? Current_Ovd_Amt { get; set; }
        public int? No_Ovd_Days { get; set; }
        public decimal? Bom_Total_Exposure { get; set; }
        public string Paid_Off { get; set; }
    }
}
