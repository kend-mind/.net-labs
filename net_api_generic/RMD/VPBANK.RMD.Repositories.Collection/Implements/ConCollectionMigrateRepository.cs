using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class ConCollectionMigrateRepository : Repository<CollectionContext, ConCollectionMigrate, long>, IConCollectionMigrateRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<ConCollectionMigrate> _logger;
        protected readonly CollectionContext _collectionContext;

        public ConCollectionMigrateRepository(IDistributedCache distributedCache, 
            ILogger<ConCollectionMigrate> logger, 
            ITrackableRepository<CollectionContext, ConCollectionMigrate, long> trackableRepository,
            CollectionContext collectionContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _collectionContext = collectionContext;
        }
    }
}
