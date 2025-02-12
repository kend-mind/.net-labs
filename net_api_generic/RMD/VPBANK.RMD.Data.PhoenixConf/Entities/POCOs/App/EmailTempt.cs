using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Email_Tempt", Schema = "App")]
    public class EmailTempt : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Email_Tempt_Name { get; set; }
        public string Content_Html { get; set; }
        public string Group_Code { get; set; }
    }

    public class EmailTemptDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public string Email_Tempt_Name { get; set; }
        public string Content_Html { get; set; }
        public string Group_Code { get; set; }
    }
}
