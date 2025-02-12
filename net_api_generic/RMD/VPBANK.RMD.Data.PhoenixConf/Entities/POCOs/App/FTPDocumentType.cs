using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("FTP_Document_Type", Schema = "App")]
    public class FTPDocumentType : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Doc_Key { get; set; }
        public string Doc_Val { get; set; }
        public string Source_System { get; set; }
        public string Segment_Code { get; set; }
        public bool Is_Active { get; set; }
    }
}
