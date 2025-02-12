using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Serializable]
    [Table("LU_Campaign", Schema = "dbo")]
    public class LuCampaign : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Campaign_Code { get; set; }
        public string Campaign_Description { get; set; }
    }

    public class LuCampaignDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public string Campaign_Code { get; set; }
        public string Campaign_Description { get; set; }
    }
}
