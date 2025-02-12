using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;

namespace VPBANK.RMD.EFCore.Implements
{
    public abstract class Repository<TContext, TEntity, TKey> : IRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly ITrackableRepository<TContext, TEntity, TKey> TrackableRepository;

        protected Repository(ITrackableRepository<TContext, TEntity, TKey> trackableRepository) => TrackableRepository = trackableRepository;

        public virtual async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default) => await TrackableRepository.FindAsync(keyValues, cancellationToken);

        public virtual async Task<TEntity> FindAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await TrackableRepository.FindAsync(keyValue, cancellationToken);

        public virtual async Task<bool> ExistsAsync(object[] keyValues, CancellationToken cancellationToken = default) => await TrackableRepository.ExistsAsync(keyValues, cancellationToken);

        public virtual async Task<bool> ExistsAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await TrackableRepository.ExistsAsync(keyValue, cancellationToken);

        public virtual async Task LoadPropertyAsync(TEntity item, Expression<Func<TEntity, object>> property, CancellationToken cancellationToken = default) => await TrackableRepository.LoadPropertyAsync(item, property, cancellationToken);

        public virtual void Attach(TEntity item) => TrackableRepository.Attach(item);

        public virtual void Detach(TEntity item) => TrackableRepository.Detach(item);

        public virtual void Insert(TEntity item) => TrackableRepository.Insert(item);

        public virtual Task InsertAsync(TEntity item) => throw new NotImplementedException();

        public virtual void Update(TEntity item) => TrackableRepository.Update(item);

        public virtual Task UpdateAsync(TEntity item) => throw new NotImplementedException();

        public virtual void Delete(TEntity item) => TrackableRepository.Delete(item);

        public virtual async Task<bool> DeleteAsync(object[] keyValues, CancellationToken cancellationToken = default) => await TrackableRepository.DeleteAsync(keyValues, cancellationToken);

        public virtual async Task<bool> DeleteAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default) => await TrackableRepository.DeleteAsync(keyValue, cancellationToken);

        public virtual IQuery<TContext, TEntity, TKey> Query() => TrackableRepository.Query();

        public virtual IQueryable<TEntity> Queryable() => TrackableRepository.Queryable();

        public virtual IQueryable<TEntity> QueryableSql(string sql, params object[] parameters) => TrackableRepository.QueryableSql(sql, parameters);

        public virtual async Task<IEnumerable<TEntity>> SelectAsync(CancellationToken cancellationToken = default) => await TrackableRepository.Query().SelectAsync(cancellationToken);

        public virtual async Task<IEnumerable<TEntity>> SelectSqlAsync(string sql, object[] parameters, CancellationToken cancellationToken = default) => await TrackableRepository.Query().SelectSqlAsync(sql, parameters, cancellationToken);

        public virtual void ApplyChanges(params TEntity[] entities) => TrackableRepository.ApplyChanges(entities);

        public virtual void AcceptChanges(params TEntity[] entities) => TrackableRepository.AcceptChanges(entities);

        public virtual void DetachEntities(params TEntity[] entities) => TrackableRepository.DetachEntities(entities);

        public virtual async Task LoadRelatedEntities(params TEntity[] entities) => await TrackableRepository.LoadRelatedEntities(entities);
    }
}
