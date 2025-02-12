using Microsoft.Extensions.Configuration;
using VPBANK.RMD.API.Settings.Sections;

namespace VPBANK.RMD.API.Settings
{
    public interface IAppSettingsReader
    {
        AppSettings GetAppSettings();
    }

    public class AppSettingsReader : IAppSettingsReader
    {
        private readonly IConfiguration _configuration;

        public AppSettingsReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppSettings GetAppSettings()
        {
            return new AppSettings
            {
                ApiUri = _configuration.GetSection(nameof(AppSettings.ApiUri)).Value,
                ApiUri_Jav = _configuration.GetSection(nameof(AppSettings.ApiUri_Jav)).Value,
                ApiSwaggerInfo = SwaggerSetting.GetSwaggerInfoSection(_configuration),
                ConnectionStrings = new ConnectionStringsSection
                {
                    AuthConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.AuthConnection)).Value,
                    PhoenixConfConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.PhoenixConfConnection)).Value,
                    PhoenixDataConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.PhoenixDataConnection)).Value,
                    CollectionConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.CollectionConnection)).Value,
                    IFRS9ConfConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.IFRS9ConfConnection)).Value,
                    IFRS9DataConnection = _configuration.GetSection("ConnectionStrings").GetSection(nameof(ConnectionStringsSection.IFRS9DataConnection)).Value
                },
                Jwt = JWTSetting.GetJwtSection(_configuration),
                RabbitMq = RabbitMqSetting.GetRabbitMqSetting(_configuration),
                ElasticAudit = new ElasticAuditSection
                {
                    Uri = _configuration["ElasticAudit:Uri"],
                    FixAlias = bool.Parse(_configuration["ElasticAudit:FixAlias"]),
                    NotiAlias = _configuration["ElasticAudit:NotiAlias"],
                    AuditAlias = _configuration["ElasticAudit:AuditAlias"],
                    IndexPerYear = bool.Parse(_configuration["ElasticAudit:IndexPerYear"]),
                    AmountOfPreviousIndicesUsedInAlias = int.Parse(_configuration["ElasticAudit:AmountOfPreviousIndicesUsedInAlias"])
                },
                FtpInfo = FtpInfoSetting.GetFtpSecionSetting(_configuration)
            };
        }
    }
}
