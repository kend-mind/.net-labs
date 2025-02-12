using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using EFCore.BulkExtensions;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Entities;
using VPBANK.RMD.EFCore.Implements;

namespace VPBANK.RMD.EFCore.Generics
{
    /// <summary>
    /// extending IRepository<TEntity> and/or ITrackableRepository<TEntity>, scope: application-wide
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDomainRepository<TContext, TEntity, TKey> : ITrackableRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        // scope: application wide for all repositories
        TContext GetContext();
        TEntity Find(object[] keyValues, CancellationToken cancellationToken = default);
        TEntity Find(TKey keyValue, CancellationToken cancellationToken = default);
        void BulkInsert(IList<object> entities, CancellationToken cancellationToken = default);
        Task BulkInsertAsync(IList<object> entities, CancellationToken cancellationToken = default);
        void BulkUpdate(IList<object> entities, CancellationToken cancellationToken = default);
        Task BulkUpdateAsync(IList<object> entities, CancellationToken cancellationToken = default);
        void BulkDelete(IList<object> entities, CancellationToken cancellationToken = default);
        Task BulkDeleteAsync(IList<object> entities, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Domain base repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DomainRepository<TContext, TEntity, TKey> : TrackableRepository<TContext, TEntity, TKey>, IDomainRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public DomainRepository(TContext context) : base(context)
        {
        }

        public TContext GetContext() => Context;

        private BulkConfig BulkConfig = new BulkConfig
        {
            PropertiesToExclude = new List<string>
            {
                nameof(RequestableEntity.ReqActionType),
                nameof(RequestableEntity.ReqObjectType),
                nameof(RequestableEntity.ReqDetailId),
                nameof(RequestableEntity.SyncId),
                nameof(RequestableEntity.TrackingState),
                nameof(RequestableEntity.ModifiedProperties),
                nameof(RequestableEntity.EntityIdentifier)
            },
            PreserveInsertOrder = true,
            SetOutputIdentity = true,
            BatchSize = 100000
        };

        // adding synchronous Find, scope: application-wide
        public TEntity Find(object[] keyValues, CancellationToken cancellationToken = default)
        {
            return this.Context.Find<TEntity>(keyValues);
        }

        public TEntity Find(TKey keyValue, CancellationToken cancellationToken = default)
        {
            return this.Context.Find<TEntity>(keyValue);
        }

        public void BulkInsert(IList<object> entities, CancellationToken cancellationToken = default)
        {
            Context.BulkInsert(typeof(TEntity), entities, BulkConfig);
        }

        public async Task BulkInsertAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            await Context.BulkInsertAsync(typeof(TEntity), entities, BulkConfig);
        }

        public void BulkUpdate(IList<object> entities, CancellationToken cancellationToken = default)
        {
            Context.BulkUpdate(typeof(TEntity), entities, BulkConfig);
        }

        public async Task BulkUpdateAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            await Context.BulkUpdateAsync(typeof(TEntity), entities, BulkConfig);
        }

        public void BulkDelete(IList<object> entities, CancellationToken cancellationToken = default)
        {
            BulkConfig.SetOutputIdentity = false;
            Context.BulkDelete(typeof(TEntity), entities, BulkConfig);
        }

        public async Task BulkDeleteAsync(IList<object> entities, CancellationToken cancellationToken = default)
        {
            BulkConfig.SetOutputIdentity = false;
            await Context.BulkDeleteAsync(typeof(TEntity), entities, BulkConfig);
        }
    }
}
