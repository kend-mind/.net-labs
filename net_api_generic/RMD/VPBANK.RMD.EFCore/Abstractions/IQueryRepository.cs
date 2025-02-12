using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VPBANK.RMD.EFCore.Entities;

namespace VPBANK.RMD.EFCore.Abstractions
{
    public interface IQueryBaseRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
        Task LoadPropertyAsync(TEntityQuery item, Expression<Func<TEntityQuery, object>> property, CancellationToken cancellationToken = default);
        IQueryable<TEntityQuery> Queryable();
        IQueryable<TEntityQuery> QueryableSql(string sql, params object[] parameters);
    }

    public interface IQueryableRepository<TContext, TEntityQuery> : IQueryBaseRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
    }

    public interface IQueryRepository<TContext, TEntityQuery> : IQueryableRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
    }
}
