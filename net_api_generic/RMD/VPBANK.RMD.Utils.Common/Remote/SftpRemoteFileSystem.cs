using Renci.SshNet;
using VPBANK.RMD.Utils.Common.Remote.Contexts;
using VPBANK.RMD.Utils.Common.Remote.FTP;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Utils.Common.Remote
{
    public class SftpRemoteFileSystem : SftpContext
    {
        private string _serverDetails;

        public SftpRemoteFileSystem(FtpSetting setting)
        {
            _serverDetails = ServerDetails(setting.Host, setting.Port, setting.Username, setting.Type);
            var connectionInfo = new ConnectionInfo(setting.Host, setting.Port, setting.Username, new PasswordAuthenticationMethod(setting.Username, setting.Password));
            _sftpClient = new SftpClient(connectionInfo);
        }

        public override string ServerDetails()
        {
            return _serverDetails;
        }

        private static string ServerDetails(string host, int port, string userName, string type = RemoteFileSystem.SFTP)
        {
            return string.Format("Type: '{3}' Host:'{0}' Port:'{1}' User:'{2}'", host, port, userName, type);
        }
    }
}
