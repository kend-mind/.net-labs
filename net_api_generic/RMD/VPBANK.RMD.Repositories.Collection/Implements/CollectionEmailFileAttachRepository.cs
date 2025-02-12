using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Repositories.Collection.Interfaces;

namespace VPBANK.RMD.Repositories.Collection.Implements
{
    public class CollectionEmailFileAttachRepository : Repository<CollectionContext, CollectionEmailFileAttach, int>, ICollectionEmailFileAttachRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<CollectionEmailFileAttach> _logger;
        protected readonly CollectionContext _context;

        public CollectionEmailFileAttachRepository(IDistributedCache distributedCache, ILogger<CollectionEmailFileAttach> logger, ITrackableRepository<CollectionContext, CollectionEmailFileAttach, int> trackableRepository,
            CollectionContext phoenixConfContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _context = phoenixConfContext;
        }

        public List<CollectionEmailFileAttach> FindAllByFkCollecEmailId(int fkCollecEmailId)
        {
            return TrackableRepository.Queryable().AsEnumerable().Where(c => c.Fk_Collection_email_Id == fkCollecEmailId).ToList();
        }
    }
}
