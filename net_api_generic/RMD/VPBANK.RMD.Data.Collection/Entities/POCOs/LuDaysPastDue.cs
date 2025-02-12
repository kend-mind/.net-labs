using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Serializable]
    [Table("LU_Days_Past_Due", Schema = "dbo")]
    public class LuDaysPastDue : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Days_Past_Due_Dpd { get; set; }
        public string Description { get; set; }
    }

    public class LuDaysPastDueDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public string Days_Past_Due_Dpd { get; set; }
        public string Description { get; set; }
    }
}
