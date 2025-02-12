using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_DPD_Manual", Schema = "dbo")]
    public class CollectionDpdManual : BaseEntity<long>
    {
        public override long Pk_Id { get; set; }

        [Key]
        [Column("CL_OS_Start_Date")]
        public DateTime Cl_Os_Start_Date { get; set; }

        [Key]
        [Column("Contract_ID")]
        public string Contract_Id { get; set; }

        [Column("DPD")]
        public int? Dpd { get; set; }
    }
}
