using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_Repay_Adj", Schema = "dbo")]
    public class CollectionRepayAdj : BaseEntity<long>
    {
        public override long Pk_Id { get; set; }

        [Key]
        [Column("Business_date")]
        public DateTime Business_Date { get; set; }

        [Key]
        [Column("Contract_ID")]
        public string Contract_Id { get; set; }

        public decimal? Repay_Adj_Amt { get; set; }
    }
}
