using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_Dead_Loan", Schema = "dbo")]
    public class CollectionDeadLoan : BaseEntity<long>
    {
        [Key]
        public override long Pk_Id { get; set; }

        [Column("Customer_Id")]
        public string Customer_Id { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? End_Date { get; set; }
    }
}
