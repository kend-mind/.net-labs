using VPBANK.RMD.Utils.AuditLog.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace VPBANK.RMD.Utils.AuditLog.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static IServiceCollection AddElasticSearch<T>(this IServiceCollection services) where T : class, IElasticLog
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return AddElasticSearch<T>(services, setupAction: null);
        }

        public static IServiceCollection AddElasticSearch<T>(this IServiceCollection services, Action<ElasticSearchOptions> setupAction) where T : class, IElasticLog
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.TryAdd(new ServiceDescriptor(typeof(IAuditElasticProvider<T>), typeof(AuditElasticProvider<T>), ServiceLifetime.Transient));
            services.TryAdd(new ServiceDescriptor(typeof(INotiElasticProvider<T>), typeof(NotiElasticProvider<T>), ServiceLifetime.Transient));

            if (setupAction != null)
                services.Configure(setupAction);

            return services;
        }
    }
}
