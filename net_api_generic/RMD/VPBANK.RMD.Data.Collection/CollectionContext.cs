using Microsoft.EntityFrameworkCore;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.Data.Collection.StoredProcedures;
using VPBANK.RMD.Data.Collection.Views;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Data.Collection
{
    public class CollectionContext : DbContext
    {
        public CollectionContext(DbContextOptions<CollectionContext> options) : base(options)
        {
        }

        // DbQuery<T> is for Stored Procedure
        public DbQuery<TableInfo> TableInfos { get; set; }
        public DbQuery<ColumnInfo> ColumnInfos { get; set; }
        public DbQuery<CollectionRepayReport> CollectionRepayReports { get; set; }
        public DbQuery<CollectionNewbookReport> CollectionNewbookReports { get; set; }

        // POCO data
        public DbSet<CollectionDeadLoan> CollectionDeadLoans { get; set; }
        public DbSet<CollectionSellLoan> CollectionSellLoans { get; set; }
        public DbSet<ConCollectionListDaily> ConCollectionListDailies { get; set; }
        public DbSet<CollectionEmail> CollectionEmails { get; set; }
        public DbSet<CollectionEmailFileAttach> CollectionEmailFileAttaches { get; set; }
        public DbSet<ViewCollectionEmail> ViewCollectionEmails { get; set; }
        public DbSet<CollectionLogEmail> CollectionLogEmails { get; set; }
        public DbSet<CollectionLogFile> CollectionLogFiles { get; set; }
        public DbSet<OsCompany> OsCompanies { get; set; }
        public DbSet<ConCollectionMigrate> ConCollectionMigrates { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<LuCampaign> LuCampaigns { get; set; }
        public DbSet<MpContractCampaign> MpContractCampaigns { get; set; }
        public DbSet<CollectionRepayAdj> CollectionRepayAdjs { get; set; }
        public DbSet<CollectionDpdManual> CollectionDpdManuals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CollectionDeadLoan>().ToTable("Collection_Dead_Loan");
            modelBuilder.Entity<CollectionDeadLoan>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionSellLoan>().ToTable("Collection_Sell_Loan");
            modelBuilder.Entity<CollectionSellLoan>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<ConCollectionListDaily>().ToTable("Con_Collection_List_Daily");
            modelBuilder.Entity<ConCollectionListDaily>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionEmail>().ToTable("Collection_Email", "dbo");
            modelBuilder.Entity<CollectionEmail>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionEmailFileAttach>().ToTable("Collection_email_File_Attach", "dbo");
            modelBuilder.Entity<CollectionEmailFileAttach>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<ViewCollectionEmail>().ToView("View_Collection_Email", "dbo");
            modelBuilder.Entity<ViewCollectionEmail>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionLogEmail>().ToView("view_Collection_log_email", "dbo");
            modelBuilder.Entity<CollectionLogEmail>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionLogFile>().ToView("view_Collection_log_file_generated", "dbo");
            modelBuilder.Entity<CollectionLogFile>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<OsCompany>().ToTable("Os_Company", "dbo");
            modelBuilder.Entity<OsCompany>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<ConCollectionMigrate>().ToTable("Con_Collection_Migrate", "dbo");
            modelBuilder.Entity<ConCollectionMigrate>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<Segment>().ToTable("Segment", "dbo");
            modelBuilder.Entity<Segment>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<LuCampaign>().ToTable("LU_Campaign", "dbo");
            modelBuilder.Entity<LuCampaign>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<MpContractCampaign>().ToTable("MP_Contract_Campaign", "dbo");
            modelBuilder.Entity<MpContractCampaign>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<LuDaysPastDue>().ToTable("LU_Days_Past_Due", "dbo");
            modelBuilder.Entity<LuDaysPastDue>()
                //.Property(item => item.Pk_Id)
                //.IsRequired(false);
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<ConfCollectionFeeRatio>().ToTable("Conf_Collection_Fee_Ratio", "dbo");
            modelBuilder.Entity<ConfCollectionFeeRatio>()
                //.Property(item => item.Pk_Id)
                //.IsRequired(false);
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<CollectionRepayAdj>().ToTable("Collection_Repay_Adj", "dbo");
            modelBuilder.Entity<CollectionRepayAdj>()
                .HasKey(item => new { item.Business_Date, item.Contract_Id });

            modelBuilder.Entity<CollectionDpdManual>().ToTable("Collection_DPD_Manual", "dbo");
            modelBuilder.Entity<CollectionDpdManual>()
                .HasKey(item => new { item.Cl_Os_Start_Date, item.Contract_Id });
        }
    }
}
