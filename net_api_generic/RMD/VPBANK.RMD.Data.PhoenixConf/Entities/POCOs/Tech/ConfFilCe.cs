using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech
{
    [Table("Conf_Fil_CE", Schema = "Tech")]
    public class ConfFilCe : BaseEntity<decimal>
    {
        [Key]
        public override decimal Pk_Id { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public string Engine_Code { get; set; }

        public string Entity_Code { get; set; }

        public string Approach { get; set; }

        public string Scenario_Id { get; set; }

        public string Version_Id { get; set; }

        public decimal Filter_Id { get; set; }

        public string Table_Name { get; set; }

        public string Field_Name { get; set; }

        public string Condition { get; set; }

        public string Value { get; set; }
    }
}
