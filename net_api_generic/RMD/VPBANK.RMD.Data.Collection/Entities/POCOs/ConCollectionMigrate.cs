using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Con_Collection_Migrate", Schema = "dbo")]
    public class ConCollectionMigrate : BaseEntity<long>
    {
        [Key]
        public override long Pk_Id { get; set; }
        public DateTime? Classify_Bb_Date { get; set; }
        public string Customer_Id { get; set; }
        public string Contract_Id { get; set; }
        public string Os_Company { get; set; }
        public DateTime Cl_Os_Start_Date { get; set; }
        public string Product_Group { get; set; }
        public string Region { get; set; }
        public string Reason { get; set; }
        public DateTime? Cl_Os_End_Date { get; set; }
        public DateTime? Cl_Os_End_Date_Fee { get; set; }
    }
}
