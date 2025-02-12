using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;

namespace VPBANK.RMD.EFCore.Abstractions
{
    public interface ITrackableRepository<TContext, TEntity, TKey> : IBaseRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        void ApplyChanges(params TEntity[] entities);
        void AcceptChanges(params TEntity[] entities);
        void DetachEntities(params TEntity[] entities);
        Task LoadRelatedEntities(params TEntity[] entities);
    }
}
