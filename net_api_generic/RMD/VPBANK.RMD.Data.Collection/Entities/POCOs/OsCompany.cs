using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Entities.POCOs
{
    [Table("Os_Company", Schema = "dbo")]
    public class OsCompany : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public string Os_Company { get; set; }
        public string Os_Company_Name { get; set; }
    }

    public class OsCompanyDto : RequestableEntity
    {
        public int? Pk_Id { get; set; }
        public string Os_Company { get; set; }
        public string Os_Company_Name { get; set; }
    }
}
