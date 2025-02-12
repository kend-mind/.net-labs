using Microsoft.EntityFrameworkCore;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Data.IFRS9_Conf
{
    public partial class IFRS9_ConfContext : DbContext
    {
        public IFRS9_ConfContext(DbContextOptions<IFRS9_ConfContext> options) : base(options)
        {
        }

        // DbQuery<T> is for Stored Procedure
        public DbQuery<TableInfo> TableInfos { get; set; }
        public DbQuery<ColumnInfo> ColumnInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
