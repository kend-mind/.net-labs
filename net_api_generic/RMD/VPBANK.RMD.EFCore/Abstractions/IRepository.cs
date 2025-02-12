using VPBANK.RMD.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using TrackableEntities.Common.Core;

namespace VPBANK.RMD.EFCore.Abstractions
{
    public interface IRepository<TContext, TEntity, TKey> : ITrackableRepository<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, ITrackable, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}
