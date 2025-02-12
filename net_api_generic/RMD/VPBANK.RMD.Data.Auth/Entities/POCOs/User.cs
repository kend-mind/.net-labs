using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;
using VPBANK.RMD.Utils.Common.Expressions;

namespace VPBANK.RMD.Data.Auth.Entities.POCOs
{
    [Table("Staff", Schema = "dbo")]
    public class User : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }

        public string U_Name { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(RegularExpressions.EMAIL, ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
