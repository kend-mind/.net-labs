using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Notification_Count", Schema = "App")]
    public class NotificationCount : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Username { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
