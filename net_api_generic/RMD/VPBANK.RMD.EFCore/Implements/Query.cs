using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore.Implements
{
    public class Query<TContext, TEntity, TKey> : IQuery<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private int? _skip;
        private int? _take;
        private IQueryable<TEntity> _query;
        private IOrderedQueryable<TEntity> _orderedQuery;

        public Query(IBaseRepository<TContext, TEntity, TKey> repository) => _query = repository.Queryable();

        private IQuery<TContext, TEntity, TKey> Set(Action<Query<TContext, TEntity, TKey>> setParameter)
        {
            setParameter(this);
            return this;
        }

        public virtual IQuery<TContext, TEntity, TKey> Skip(int skip) => Set(q => q._skip = skip);
        public virtual IQuery<TContext, TEntity, TKey> Take(int take) => Set(q => q._take = take);
        public virtual async Task<IEnumerable<TEntity>> SelectAsync(CancellationToken cancellationToken = default)
        {
            _query = _orderedQuery ?? _query;

            if (_skip.HasValue) _query = _query.Skip(_skip.Value);
            if (_take.HasValue) _query = _query.Take(_take.Value);

            return await _query.ToListAsync(cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> SelectSqlAsync(string sql, object[] parameters, CancellationToken cancellationToken = default) => await (_query as DbSet<TEntity>)?.FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
        public virtual IQuery<TContext, TEntity, TKey> Where(Expression<Func<TEntity, bool>> predicate) => Set(q => q._query = q._query.Where(predicate));
        public virtual IQuery<TContext, TEntity, TKey> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty) => Set(q => q._query = q._query.Include(navigationProperty));
        public virtual IQuery<TContext, TEntity, TKey> Include(string navigationPropertyPath) => Set(q => q._query = q._query.Include(navigationPropertyPath));
        public virtual IQuery<TContext, TEntity, TKey> OrderBy(Expression<Func<TEntity, object>> keySelector)
        {
            if (_orderedQuery == null) _orderedQuery = _query.OrderBy(keySelector);
            else _orderedQuery.OrderBy(keySelector);
            return this;
        }
        public virtual IQuery<TContext, TEntity, TKey> OrderByDescending(Expression<Func<TEntity, object>> keySelector)
        {
            if (_orderedQuery == null) _orderedQuery = _query.OrderByDescending(keySelector);
            else _orderedQuery.OrderByDescending(keySelector);
            return this;
        }
        public virtual IQuery<TContext, TEntity, TKey> ThenBy(Expression<Func<TEntity, object>> thenBy) => Set(q => q._orderedQuery.ThenBy(thenBy));
        public virtual IQuery<TContext, TEntity, TKey> ThenByDescending(Expression<Func<TEntity, object>> thenByDescending) => Set(q => q._orderedQuery.ThenByDescending(thenByDescending));
        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default) => await _query.CountAsync(cancellationToken);
        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default) => await _query.FirstOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await _query.FirstOrDefaultAsync(predicate, cancellationToken);
        public virtual async Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default) => await _query.SingleOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await _query.SingleOrDefaultAsync(predicate, cancellationToken);
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await _query.AnyAsync(predicate, cancellationToken);
        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default) => await _query.AnyAsync(cancellationToken);
        public virtual async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await _query.AllAsync(predicate, cancellationToken);
    }
}
