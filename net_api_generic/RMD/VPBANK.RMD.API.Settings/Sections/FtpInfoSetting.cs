using Microsoft.Extensions.Configuration;
using VPBANK.RMD.Utils.Common.Remote.FTP;
using static VPBANK.RMD.API.Settings.AppSettings;

namespace VPBANK.RMD.API.Settings.Sections
{
    public static class FtpInfoSetting
    {
        public static FtpInfoSection GetFtpSecionSetting(IConfiguration configuration)
        {
            return new FtpInfoSection
            {
                Host = configuration["FtpInfo:Host"],
                Port = int.Parse(configuration["FtpInfo:Port"]),
                Username = configuration["FtpInfo:Username"],
                Password = configuration["FtpInfo:Password"],
                FolderReq = configuration["FtpInfo:FolderReq"]
            };
        }

        public static FtpSetting GetFtpSetting(IConfiguration configuration)
        {
            var ftpConf = GetFtpSecionSetting(configuration);
            var setting = new FtpSetting
            {
                Host = ftpConf.Host,
                Port = ftpConf.Port,
                Username = ftpConf.Username,
                Password = ftpConf.Password,
                UploadFolder = ftpConf.FolderReq
            };
            return setting;
        }
    }
}
