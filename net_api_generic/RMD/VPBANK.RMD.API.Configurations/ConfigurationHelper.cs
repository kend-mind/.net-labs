using Microsoft.Extensions.Configuration;
using System.IO;

namespace VPBANK.RMD.API.Configurations
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(string env, string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(string.IsNullOrEmpty(outputPath) ? Directory.GetCurrentDirectory() : outputPath)
                .AddJsonFile(path: $"appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: $"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static T GetApplicationConfiguration<T>(string outputPath, IConfigurationRoot iConfig)
        {
            var configuration = default(T);
            iConfig?.GetSection(typeof(T).Name)?.Bind(configuration);
            return configuration;
        }

        public static IConfigurationSection GetConfigurationSection<T>(IConfigurationRoot iConfig)
        {
            return iConfig?.GetSection(typeof(T).Name);
        }
    }
}
