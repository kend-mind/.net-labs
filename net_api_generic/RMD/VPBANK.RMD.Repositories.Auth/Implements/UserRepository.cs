using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.Data.Auth.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.Repositories.Auth.Interfaces;

namespace VPBANK.RMD.Repositories.Auth.Implements
{
    public class UserRepository : Repository<AuthContext, User, int>, IUserRepository
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger<User> _logger;
        protected readonly AuthContext _authContext;

        public UserRepository(IDistributedCache distributedCache, ILogger<User> logger, ITrackableRepository<AuthContext, User, int> trackableRepository,
            AuthContext authContext) : base(trackableRepository)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _authContext = authContext;
        }
    }
}
