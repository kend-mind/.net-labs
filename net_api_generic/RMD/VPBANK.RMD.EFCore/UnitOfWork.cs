using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        protected DbContext Context { get; }
        private bool _disposed;

        public UnitOfWork(TContext context)
        {
            Context = context;
            _disposed = false;
        }

        public TContext GetDbContext() => (TContext)Context;

        public virtual int SaveChanges() => Context.SaveChanges();

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await Context.SaveChangesAsync(cancellationToken);

        public virtual int ExecuteSqlRaw(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default) => Context.Database.ExecuteSqlRaw(sql, parameters, cancellationToken);

        public virtual int ExecuteSqlRaw(string sql, params object[] parameters) => Context.Database.ExecuteSqlRaw(sql, parameters);

        public virtual async Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default) => await Context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

        public virtual void BeginTransaction() => Context.Database.BeginTransaction();

        public virtual Task BeginTransactionAsync(CancellationToken cancellationToken = default) => Context.Database.BeginTransactionAsync(cancellationToken);

        public virtual void Commit() => Context.Database.CommitTransaction();

        public virtual void Rollback() => Context.Database.RollbackTransaction();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool isDisposing)
        {
            if (!_disposed && isDisposing) Context.Dispose();
            _disposed = true;
        }
    }
}
