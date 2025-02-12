using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Serializable]
    [Table("Segment", Schema = "dbo")]
    public class Segment : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Segment_Name { get; set; }
        public string Segment_Desc { get; set; }
    }
}
