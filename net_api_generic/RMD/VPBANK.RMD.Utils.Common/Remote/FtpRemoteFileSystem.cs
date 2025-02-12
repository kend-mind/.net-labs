using System.Net;
using FluentFTP;
using VPBANK.RMD.Utils.Common.Remote.Contexts;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Utils.Common.Remote.FTP
{
    public class FtpRemoteFileSystem : FtpContext
    {
        private string _serverDetails;

        public FtpRemoteFileSystem(FtpSetting setting)
        {
            _serverDetails = ServerDetails(setting.Host, setting.Port, setting.Username, setting.Type);
            _ftpClient = new FtpClient(setting.Host)
            {
                Credentials = new NetworkCredential(setting.Username, setting.Password),
                Port = setting.Port
            };
        }

        public override string ServerDetails()
        {
            return _serverDetails;
        }

        private static string ServerDetails(string host, int port, string userName, string type = RemoteFileSystem.FTP)
        {
            return string.Format("Type: '{3}' Host:'{0}' Port:'{1}' User:'{2}'", host, port, userName, type);
        }
    }
}
