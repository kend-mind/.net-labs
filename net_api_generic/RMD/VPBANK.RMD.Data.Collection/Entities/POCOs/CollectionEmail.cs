using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Collection_Email", Schema = "dbo")]
    public class CollectionEmail : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Os_Company { get; set; }
        public string Segment { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public int Fk_Email_Tempt_Id { get; set; }
        public string Recipient_Email { get; set; }
    }
}
