using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Serializable]
    [Table("Conf_Collection_Fee_Ratio", Schema = "dbo")]
    public class ConfCollectionFeeRatio : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string Not_Bad_Debt_Flag { get; set; }
        public string Dpd_Before_Pmt_Date { get; set; }
        public string Sell_Loan { get; set; }
        public string Dead_Loan { get; set; }
        public string Dpd_Before_Cl_Os_Start_Date { get; set; }
        public string Product { get; set; }
        public string Os_Company { get; set; }
        public decimal? Fee_Ratio { get; set; }
    }

    public class ConfCollectionFeeRatioDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string Not_Bad_Debt_Flag { get; set; }
        public string Dpd_Before_Pmt_Date { get; set; }
        public string Sell_Loan { get; set; }
        public string Dead_Loan { get; set; }
        public string Dpd_Before_Cl_Os_Start_Date { get; set; }
        public string Product { get; set; }
        public string Os_Company { get; set; }
        public decimal? Fee_Ratio { get; set; }
    }
}
