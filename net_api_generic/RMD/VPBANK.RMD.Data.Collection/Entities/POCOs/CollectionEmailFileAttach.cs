using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_email_File_Attach", Schema = "dbo")]
    public class CollectionEmailFileAttach : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public int Fk_Collection_email_Id { get; set; }
        public int Fk_File_Attach_Id { get; set; }
    }
}
