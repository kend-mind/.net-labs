using Microsoft.EntityFrameworkCore;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Data.IFRS9_Data
{
    public partial class IFRS9_DataContext : DbContext
    {
        public IFRS9_DataContext(DbContextOptions<IFRS9_DataContext> options) : base(options)
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
