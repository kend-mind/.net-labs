using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Views.WebCore
{
    [Table("View_Conf_Table_Mapping", Schema = "Web_Core")]
    public class ViewConfTableMapping : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }

        public string Engine_Code { get; set; }

        public int Fk_Table_Definition_Id { get; set; }

        public string Table_Name { get; set; }

        public string Table_Description { get; set; }

        public string Link { get; set; }

        public string Link_Text { get; set; }
    }
}
