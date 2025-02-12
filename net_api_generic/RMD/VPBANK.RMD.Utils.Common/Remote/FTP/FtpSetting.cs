namespace VPBANK.RMD.Utils.Common.Remote.FTP
{
    public class FtpSetting
    {
        public string Host { set; get; }
        public int Port { set; get; } = 21;
        public string Type { set; get; } = "FTP";
        public string Username { set; get; }
        public string Password { set; get; }
        public string UploadFolder { set; get; }
    }
}
