using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore.Implements
{
    public class BaseRepository<TContext, TEntity, TKey> : IBaseRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TContext Context;
        public DbSet<TEntity> Set { get; }

        public BaseRepository(TContext context)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Context = context;
            Context.Database.SetCommandTimeout(1200);
            Set = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> FindAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await FindAsync(new object[] { keyValue }, cancellationToken);
        public virtual async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default) => await Set.FindAsync(keyValues, cancellationToken);
        public virtual async Task<bool> ExistsAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await ExistsAsync(new object[] { keyValue }, cancellationToken);
        public virtual async Task<bool> ExistsAsync(object[] keyValues, CancellationToken cancellationToken = default) => await FindAsync(keyValues, cancellationToken) != null;
        public virtual async Task LoadPropertyAsync(TEntity item, Expression<Func<TEntity, object>> property, CancellationToken cancellationToken = default) => await Context.Entry(item).Reference(property).LoadAsync(cancellationToken);
        public virtual void Attach(TEntity item) => Set.Attach(item);
        public virtual void Detach(TEntity item) => Context.Entry(item).State = EntityState.Detached;
        public virtual void Insert(TEntity item) => Context.Entry(item).State = EntityState.Added;
        public virtual Task InsertAsync(TEntity item) => throw new NotImplementedException();
        public virtual void Update(TEntity item) => Context.Entry(item).State = EntityState.Modified;
        public virtual Task UpdateAsync(TEntity item) => throw new NotImplementedException();
        public virtual void Delete(TEntity item) => Context.Entry(item).State = EntityState.Deleted;
        public virtual async Task<bool> DeleteAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await DeleteAsync(new object[] { keyValue }, cancellationToken);
        public virtual async Task<bool> DeleteAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(keyValues, cancellationToken);
            if (item == null) return false;
            Context.Entry(item).State = EntityState.Deleted;
            return true;
        }
        public virtual IQueryable<TEntity> Queryable() => Set;
        public virtual IQueryable<TEntity> QueryableSql(string sql, params object[] parameters) => Set.FromSqlRaw(sql, parameters);
        public virtual IQuery<TContext, TEntity, TKey> Query() => new Query<TContext, TEntity, TKey>(this);
    }
}
