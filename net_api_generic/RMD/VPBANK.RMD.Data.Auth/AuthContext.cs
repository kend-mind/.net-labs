using VPBANK.RMD.Data.Auth.Entities.POCOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.Data.Auth
{
    public class AuthContext : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor;

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public AuthContext(DbContextOptions<AuthContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        // POCO

        // DbQuery<T> is for Stored Procedure
        public DbQuery<User> UsersQuery { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Staff", "dbo");
            modelBuilder.Entity<User>().HasKey(item => new { item.Pk_Id });
        }

        public override int SaveChanges()
        {
            var now = DateTime.Now;

            // Get all the entities that inherit from AuditableEntity and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            if (entries != null)
                foreach (var entityEntry in entries)
                {
                    // If the entity state is Added let's set the CreatedAt and CreatedBy properties
                    if (entityEntry.State == EntityState.Added)
                    {
                        ((AuditableEntity)entityEntry.Entity).Created_At = now;
                        ((AuditableEntity)entityEntry.Entity).Created_By = /*this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ??*/ "MyApp";
                    }
                    else
                    {
                        // If the state is Modified then we don't want to modify the CreatedAt and CreatedBy properties so we set their state as IsModified to false
                        Entry((AuditableEntity)entityEntry.Entity).Property(p => p.Created_At).IsModified = false;
                        Entry((AuditableEntity)entityEntry.Entity).Property(p => p.Created_By).IsModified = false;
                    }

                // In any case we always want to set the properties ModifiedAt and ModifiedBy
                ((AuditableEntity)entityEntry.Entity).Modified_At = now;
                    ((AuditableEntity)entityEntry.Entity).Modified_By = /*this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ??*/ "MyApp";
                }

            // After we set all the needed properties we call the base implementation of SaveChanges to actually save our entities in the database
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var now = DateTime.Now;

            // Get all the entities that inherit from AuditableEntity and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            if (entries != null)
                foreach (var entityEntry in entries)
                {
                    // If the entity state is Added let's set the CreatedAt and CreatedBy properties
                    if (entityEntry.State == EntityState.Added)
                    {
                        ((AuditableEntity)entityEntry.Entity).Created_At = now;
                        ((AuditableEntity)entityEntry.Entity).Created_By = /*this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ??*/ "VBP_MRD_Administrator";
                    }
                    else
                    {
                        // If the state is Modified then we don't want to modify the CreatedAt and CreatedBy properties so we set their state as IsModified to false
                        Entry((AuditableEntity)entityEntry.Entity).Property(p => p.Created_At).IsModified = false;
                        Entry((AuditableEntity)entityEntry.Entity).Property(p => p.Created_By).IsModified = false;
                    }

                // In any case we always want to set the properties ModifiedAt and ModifiedBy
                ((AuditableEntity)entityEntry.Entity).Modified_At = now;
                    ((AuditableEntity)entityEntry.Entity).Modified_By = /*this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ??*/ "VBP_MRD_Administrator";
                }

            // After we set all the needed properties we call the base implementation of SaveChangesAsync to actually save our entities in the database
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
