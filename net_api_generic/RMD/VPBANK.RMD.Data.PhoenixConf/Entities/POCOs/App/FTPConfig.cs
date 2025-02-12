using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("FTP_Config", Schema = "App")]
    public class FTPConfig : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Source_System { get; set; }

        public string Ftp_Address { get; set; }

        public string Ftp_User { get; set; }

        public string Ftp_Password { get; set; }

        public string Upload_Folder { get; set; }

        public bool Is_Active { get; set; }

        public int Source_Code { get; set; }
    }
}
