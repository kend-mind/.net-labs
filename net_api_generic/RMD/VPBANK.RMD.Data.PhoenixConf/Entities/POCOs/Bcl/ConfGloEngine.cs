using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech
{
    [Table("conf_glo_engine", Schema = "bcl")]
    public class ConfGloEngine : BaseEntity<decimal>
    {
        [Key]
        public override decimal Pk_Id { get; set; }

        public string Param_Name { get; set; }

        public string Param_Val { get; set; }

        public string Param_Desc { get; set; }
    }
}
