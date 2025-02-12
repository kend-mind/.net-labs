using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Subscriber_Info", Schema = "App")]
    public class SubscriberInfo : BaseEntity<decimal>
    {
        [Key]
        public override decimal Pk_Id { get; set; }

        public string Username { get; set; }

        public string Routing_Key { get; set; }

        public DateTime Created_Dt { get; set; } = DateTime.Now;
    }
}
