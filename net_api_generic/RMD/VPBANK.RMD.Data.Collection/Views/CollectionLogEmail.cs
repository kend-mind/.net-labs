using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Collection.Views
{
    [Table("view_Collection_log_email", Schema = "dbo")]
    public class CollectionLogEmail : BaseEntity<int>
    {
        [Key]
        public override int Pk_Id { get; set; }
        public DateTime Send_Time { get; set; }
        public DateTime Business_Date { get; set; }
        public string Segment { get; set; }
        public string Os_Company { get; set; }
        public string Report_File_Name { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}
