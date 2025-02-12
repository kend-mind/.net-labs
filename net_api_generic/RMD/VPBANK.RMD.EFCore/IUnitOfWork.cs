using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext GetDbContext();

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int ExecuteSqlRaw(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);

        int ExecuteSqlRaw(string sql, params object[] parameters);

        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);

        void BeginTransaction();

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        void Commit();

        void Rollback();
    }
}
