using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Approve_Status", Schema = "App")]
    public class ApproveStatus : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }

        public int Fk_Request_Id { get; set; }

        public string Approver { get; set; }

        public int Step { get; set; }

        public bool Is_End { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public DateTime Created_Dt { get; set; }
    }
}
