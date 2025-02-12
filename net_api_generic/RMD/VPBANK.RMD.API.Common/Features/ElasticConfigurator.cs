using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VPBANK.RMD.Utils.AuditLog.Extensions;
using VPBANK.RMD.Utils.AuditLog.Models;
using VPBANK.RMD.Utils.Notification;

namespace VPBANK.RMD.API.Common.Features
{
    public static class ElasticConfigurator
    {
        public static IServiceCollection AddElasticSearchConf(this IServiceCollection services, IConfiguration configuration)
        {
            // Elastic search
            var uri = configuration["ElasticAudit:Uri"];
            var fixAlias = bool.Parse(configuration["ElasticAudit:FixAlias"]);
            var notiAlias = configuration["ElasticAudit:NotiAlias"];
            var auditAlias = configuration["ElasticAudit:AuditAlias"];
            var indexPerYear = bool.Parse(configuration["ElasticAudit:IndexPerYear"]);
            var amount = int.Parse(configuration["ElasticAudit:AmountOfPreviousIndicesUsedInAlias"]);
            services.AddElasticSearch<ElasticLog>(options => options.UseSettings(uri, fixAlias, notiAlias, auditAlias, indexPerYear, amount));
            services.AddElasticSearch<AuditLog>(options => options.UseSettings(uri, fixAlias, notiAlias, auditAlias, indexPerYear, amount));
            services.AddElasticSearch<NotificationHistory>(options => options.UseSettings(uri, fixAlias, notiAlias, auditAlias, indexPerYear, amount));
            return services;
        }
    }
}
