using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VPBANK.RMD.EFCore.Entities;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.EFCore.Implements
{
    public class QueryBaseRepository<TContext, TEntityQuery> : IQueryBaseRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
        private DbContext Context { get; }
        private DbSet<TEntityQuery> Set { get; }

        public QueryBaseRepository(DbContext context)
        {
            Context = context;
            Set = context.Set<TEntityQuery>();
        }

        public virtual async Task LoadPropertyAsync(TEntityQuery item, Expression<Func<TEntityQuery, object>> property, CancellationToken cancellationToken = default) => await Context.Entry(item).Reference(property).LoadAsync(cancellationToken);
        public virtual IQueryable<TEntityQuery> Queryable() => Set;
        public virtual IQueryable<TEntityQuery> QueryableSql(string sql, params object[] parameters) => Set.FromSqlRaw(sql, parameters);
    }

    public class QueryableRepository<TContext, TEntityQuery> : QueryBaseRepository<TContext, TEntityQuery>, IQueryableRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
        public QueryableRepository(TContext context) : base(context)
        {
        }
    }

    public abstract class QueryRepository<TContext, TEntityQuery> : IQueryRepository<TContext, TEntityQuery>
        where TContext : DbContext
        where TEntityQuery : BaseQueryResult
    {
        protected readonly IQueryableRepository<TContext, TEntityQuery> QueryableRepository;

        public QueryRepository(IQueryableRepository<TContext, TEntityQuery> queryableRepository)
        {
            QueryableRepository = queryableRepository;
        }

        public virtual async Task LoadPropertyAsync(TEntityQuery item, Expression<Func<TEntityQuery, object>> property, CancellationToken cancellationToken = default) => await QueryableRepository.LoadPropertyAsync(item, property, cancellationToken);
        public virtual IQueryable<TEntityQuery> Queryable() => QueryableRepository.Queryable();
        public IQueryable<TEntityQuery> QueryableSql(string sql, params object[] parameters) => QueryableRepository.QueryableSql(sql, parameters);
    }
}
