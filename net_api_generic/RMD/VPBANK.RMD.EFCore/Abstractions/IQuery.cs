using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore.Abstractions
{
    public interface IQuery<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IQuery<TContext, TEntity, TKey> Skip(int skip);
        IQuery<TContext, TEntity, TKey> Take(int take);
        Task<IEnumerable<TEntity>> SelectAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectSqlAsync(string sql, object[] parameters, CancellationToken cancellationToken = default);
        IQuery<TContext, TEntity, TKey> Where(Expression<Func<TEntity, bool>> predicate);
        IQuery<TContext, TEntity, TKey> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty);
        IQuery<TContext, TEntity, TKey> Include(string navigationPropertyPath);
        IQuery<TContext, TEntity, TKey> OrderBy(Expression<Func<TEntity, object>> keySelector);
        IQuery<TContext, TEntity, TKey> OrderByDescending(Expression<Func<TEntity, object>> keySelector);
        IQuery<TContext, TEntity, TKey> ThenBy(Expression<Func<TEntity, object>> thenBy);
        IQuery<TContext, TEntity, TKey> ThenByDescending(Expression<Func<TEntity, object>> thenByDescending);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
