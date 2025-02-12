using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Con_Collection_List_Daily", Schema = "dbo")]
    public class ConCollectionListDaily : BaseEntity<long>
    {
        [Key]
        public override long Pk_Id { get; set; }
        public DateTime Business_Date { get; set; }
        public DateTime? Classify_BB_Date { get; set; }
        public string Contract_Id { get; set; }
        public string Customer_Id { get; set; }
        public string Os_Company { get; set; }
        public DateTime CL_OS_Start_Date { get; set; }
        public string Product_Group { get; set; }
        public string Region { get; set; }
        public DateTime? CL_OS_End_Date { get; set; }
        public DateTime? CL_OS_End_Date_Fee { get; set; }
        public string Reason { get; set; }
        public string Closing_Month { get; set; }
        public string Note_Transfer { get; set; }
    }
}
