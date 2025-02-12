using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Email_File_Attach", Schema = "App")]
    public class EmailFileAttach : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string File_Name { get; set; }
        public string File_Name_Full { get; set; }
        public string Data_Source_Type { get; set; }
        public string Report_Code { get; set; }
        public string Additional_Params { get; set; }
        public string Data_Query { get; set; }
        public string File_Type { get; set; }
        public string Group_Code { get; set; }
    }
}