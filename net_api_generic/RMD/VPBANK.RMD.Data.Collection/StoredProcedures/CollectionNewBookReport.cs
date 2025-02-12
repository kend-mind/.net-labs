using System;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.StoredProcedures
{
    public class CollectionNewbookReport : BaseQueryResult
    {
        public DateTime? Business_Date { get; set; }
        public DateTime? Ngay_Chuyen_Bb { get; set; }
        public string Cif { get; set; }
        public string Contract_No { get; set; }
        public string Os_Company { get; set; }
        public DateTime? Ngay_Pl_Os { get; set; }
        public DateTime? Ngay_Ket_Thuc { get; set; }
        public string Product_Group { get; set; }
        public string Region { get; set; }
        public string Customer_Name { get; set; }
        public string Legal_Id { get; set; }

        public decimal? Original_amt { get; set; }
        public decimal? Current_Total_Exposure { get; set; }
        public decimal? Current_Ovd_Amt { get; set; }
        public decimal? Bom_Total_Exposure { get; set; }

        public decimal? Duno_Goc { get; set; }
        public decimal? Duno_Lai { get; set; }

        public decimal? Current_Outstanding { get; set; }
        public decimal? Current_PR_Amt { get; set; }
        public decimal? Current_In_Amt { get; set; }
        public decimal? Current_Pe_Amt { get; set; }
        public decimal? Current_Ps_Amt { get; set; }
        public decimal? Int_Todate { get; set; }
        public int? Current_No_Ovd_Days { get; set; }

        public string So_Tai_Khoan { get; set; }
        public DateTime? Value_Date { get; set; }
        public DateTime? Maturity_Date { get; set; }
        public decimal? Rate { get; set; }
        public string Add_On_Loan { get; set; }
        public string Term { get; set; }
        public string Branch_Id { get; set; }
        public string Branch_Name { get; set; }
        public string Ct_Uudai_Vay { get; set; }
        public DateTime? Birth_Date { get; set; }
        public string Gender { get; set; }
        public string Multi_Loan { get; set; }
        public string Product_Group_Card { get; set; }
        public string Ovd_Level { get; set; }
        public string City { get; set; }
        public string Chi_Nhanh_Mo_Tai_Khoan { get; set; }
        public string Co_Code { get; set; }
        public string Ban_No_Glx { get; set; }
        public string Ma_Ho_So { get; set; }

        public decimal? Add_On { get; set; }
        public decimal? Repay_Amt_Jul { get; set; }
        public decimal? Pr_Repay_Jul { get; set; }
        public decimal? In_Repay_Jul { get; set; }
    }
}
