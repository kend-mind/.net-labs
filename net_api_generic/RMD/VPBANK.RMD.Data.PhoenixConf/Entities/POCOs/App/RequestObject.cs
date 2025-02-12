using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App
{
    [Table("Request_Object", Schema = "App")]
    public class RequestObject : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Username { get; set; }
        public byte[] Payload { get; set; }
        public string Payload_Clazz { get; set; }
        public string Comment { get; set; }
        public string Params { get; set; }
        public string Type { get; set; }
        public string End_Point { get; set; }
        public DateTime Created_Dt { get; set; }
        public string Approve_Uri_Api { get; set; }
    }
}
