using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Views
{
    [Table("View_Collection_Email", Schema = "dbo")]
    public class ViewCollectionEmail : BaseEntity<int>
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

        public string Email_Tempt_Content { get; set; }
        public string Collection_Email_File_Attach_Name { get; set; }
        public string Fk_Collection_Email_File_Attach { get; set; }
    }

    public class ViewCollectionEmailDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public string Os_Company { get; set; }
        public string Segment { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public int Fk_Email_Tempt_Id { get; set; }
        public string Recipient_Email { get; set; }

        public string Email_Tempt_Content { get; set; }
        public string Collection_Email_File_Attach_Name { get; set; }
        public string Fk_Collection_Email_File_Attach { get; set; }
    }
}
