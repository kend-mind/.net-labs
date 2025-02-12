using Microsoft.Extensions.Configuration;

namespace VPBANK.RMD.Utils.Common.Remote.Contexts
{
    public class AppFtpConfig
    {
        public IConfigurationRoot Config { get; private set; }

        private static AppFtpConfig _instance = null;

        public static AppFtpConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppFtpConfig();
                }
                return _instance;
            }
        }

        private AppFtpConfig()
        {
            Config = Build();
        }

        private static IConfigurationRoot Build()
        {
            var builder = new ConfigurationBuilder();
            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
