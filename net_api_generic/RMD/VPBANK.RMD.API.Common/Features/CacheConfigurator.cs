using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Caches;

namespace VPBANK.RMD.API.Common.Features
{
    public static class CacheConfigurator
    {
        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Cache Distributed
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.Configure<CacheConfiguration>(options => options.GetSettings(int.Parse(configuration["DistributedCache:AbsoluteExpirationInHours"]), int.Parse(configuration["DistributedCache:SlidingExpirationInMinutes"])));

            services.AddTransient<MemoryCacheService>();
            services.AddTransient<RedisCacheService>();
            services.AddTransient<Func<CacheType, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheType.Memory:
                        return serviceProvider.GetService<MemoryCacheService>();
                    case CacheType.Redis:
                        return serviceProvider.GetService<RedisCacheService>();
                    default:
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });
            return services;
        }
    }
}
