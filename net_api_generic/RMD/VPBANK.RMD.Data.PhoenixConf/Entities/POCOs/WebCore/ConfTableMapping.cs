using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.WebCore
{
    [Table("Conf_Table_Mapping", Schema = "Web_Core")]
    public class ConfTableMapping : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Engine_Code { get; set; }
        public int Fk_Table_Definition_Id { get; set; }
        public string Mod_Id { get; set; }
    }
}