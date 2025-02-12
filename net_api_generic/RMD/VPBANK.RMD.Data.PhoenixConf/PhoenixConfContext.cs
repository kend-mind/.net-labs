using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.Tech;
using Microsoft.EntityFrameworkCore;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.Data.PhoenixConf.Views.WebCore;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.WebCore;
using VPBANK.RMD.Data.PhoenixConf.Functions;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;

namespace VPBANK.RMD.Data.PhoenixConf
{
    public partial class PhoenixConfContext : DbContext
    {
        public PhoenixConfContext(DbContextOptions<PhoenixConfContext> options) : base(options)
        {
        }

        // DbQuery<T> is for Stored Procedure
        public DbQuery<AvaiableDate> AvaiableDates { get; set; }
        public DbQuery<TableInfo> TableInfos { get; set; }
        public DbQuery<ColumnInfo> ColumnInfos { get; set; }

        // POCO data
        #region APP

        public virtual DbSet<ApproveStatus> ApproveStatus { get; set; }
        public virtual DbSet<RequestObject> RequestObjects { get; set; }
        public virtual DbSet<EmailTempt> EmailTempts { get; set; }
        public virtual DbSet<EmailFileAttach> EmailFileAttaches { get; set; }
        public virtual DbSet<FTPConfig> FTPConfigs { get; set; }
        public virtual DbSet<FTPDocumentType> FTPDocumentTypes { get; set; }
        public virtual DbSet<NotificationCount> NotificationCounts { get; set; }
        public virtual DbSet<SubscriberInfo> SubscriberInfos { get; set; }

        #endregion

        #region APP

        public virtual DbSet<ConfGloEngine> ConfGloEngines { get; set; }

        #endregion

        #region WEB_CORE

        public virtual DbSet<ConfTableMapping> ConfTableParameterMappings { get; set; }

        #endregion

        #region TECH

        public virtual DbSet<ConfFilCe> ConfFilCes { get; set; }

        #endregion

        #region VIEWS

        public virtual DbSet<ViewConfTableMapping> ViewConfTableMappings { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region APP

            modelBuilder.Entity<ApproveStatus>().ToTable("Approve_Status", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<ApproveStatus>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<RequestObject>().ToTable("Request_Object", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<RequestObject>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<EmailTempt>().ToTable("Email_Tempt", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<EmailTempt>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<EmailFileAttach>().ToTable("Email_File_Attach", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<EmailFileAttach>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<FTPConfig>().ToTable("FTP_Config", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<FTPConfig>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<FTPDocumentType>().ToTable("FTP_Document_Type", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<FTPDocumentType>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<SubscriberInfo>().ToTable("Subscriber_Info", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<SubscriberInfo>()
                .HasKey(item => new { item.Pk_Id });

            modelBuilder.Entity<NotificationCount>().ToTable("Notification_Count", PhoenixConfDbConstants.APP);
            modelBuilder.Entity<NotificationCount>()
                .HasKey(item => new { item.Pk_Id });

            #endregion

            #region WEB_CORE

            modelBuilder.Entity<ConfGloEngine>().ToTable("conf_glo_engine", PhoenixConfDbConstants.BCL);
            modelBuilder.Entity<ConfGloEngine>()
                .HasKey(item => new { item.Pk_Id });

            #endregion

            #region WEB_CORE

            modelBuilder.Entity<ConfTableMapping>().ToTable("Conf_Table_Mapping", PhoenixConfDbConstants.WEB_CORE);
            modelBuilder.Entity<ConfTableMapping>()
                .HasKey(item => new { item.Pk_Id });

            #endregion

            #region TECH

            modelBuilder.Entity<ConfFilCe>().ToTable("Conf_Fil_CE", PhoenixConfDbConstants.TECH);
            modelBuilder.Entity<ConfFilCe>()
                .HasKey(item => new { item.Pk_Id });

            #endregion

            #region VIEWS WEB_CORE

            modelBuilder.Entity<ViewConfTableMapping>().ToTable("View_Conf_Table_Mapping", PhoenixConfDbConstants.WEB_CORE);
            modelBuilder.Entity<ViewConfTableMapping>()
                .HasKey(item => new { item.Pk_Id });

            #endregion
        }
    }
}
