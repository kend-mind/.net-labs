using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Serializable]
    [Table("MP_Contract_Campaign", Schema = "dbo")]
    public class MpContractCampaign : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Type { get; set; }
        public string Mapping_Id { get; set; }
        public string Campaign_Code { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
    }
}
