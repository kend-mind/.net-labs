using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core
{
    [Table("OCE_Predeal_Data", Schema = "Core")]
    public class OcePredealData : BaseEntity<long>
    {
        [Key]
        public override long Pk_Id { get; set; }
        public long Upload_Id { get; set; }
        public DateTime Business_Date { get; set; }
        public string Customer_Id { get; set; }
        public string Contract_Id { get; set; }
        public string Amortization_Type { get; set; }
        public string Rst_Type { get; set; }
        public bool Rst_Prin_Overdue_Flag { get; set; }
        public DateTime Maturity_Date_New { get; set; }
        public int Number_Defer_Periods { get; set; }
        public bool New_Contract_Id_Flag { get; set; }
        public DateTime First_Prin_Pmt_Date { get; set; }
        public DateTime Last_Prin_Pmt_Date { get; set; }
        public int Prin_Pmt_Duration_Month { get; set; }
        public DateTime Grace_Period_End_Date { get; set; }
        public bool Flag_Covid { get; set; }
        public bool Flag_Rst { get; set; }
        public bool Flag_Pending { get; set; }
    }
}