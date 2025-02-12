using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_Sell_Loan", Schema = "dbo")]
    public class CollectionSellLoan : BaseEntity<long>
    {
        [Key]
        public override long Pk_Id { get; set; }

        public string Contract_Id { get; set; }

        public string Os_Company { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? End_Date { get; set; }
    }
}
