using Microsoft.EntityFrameworkCore;
using VPBANK.RMD.Data.PhoenixData.Entities.POCOs.Core;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Data.PhoenixData
{
    public class PhoenixDataContext : DbContext
    {
        public PhoenixDataContext(DbContextOptions<PhoenixDataContext> options) : base(options)
        {
        }

        // DbQuery<T> is for Stored Procedure
        public DbQuery<TableInfo> TableInfos { get; set; }
        public DbQuery<ColumnInfo> ColumnInfos { get; set; }

        // POCOs
        #region CORE
        public virtual DbSet<OcePredealData> OcePredealDatas { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OcePredealData>().ToTable("OCE_Predeal_Data", "Core");
            modelBuilder.Entity<OcePredealData>()
                .HasKey(item => new { item.Pk_Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
