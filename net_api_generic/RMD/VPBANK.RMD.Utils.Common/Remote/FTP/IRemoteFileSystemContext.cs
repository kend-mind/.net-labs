using FluentFTP;
using System;
using System.IO;

namespace VPBANK.RMD.Utils.Common.Remote.FTP
{
    public interface IRemoteFileSystemContext : IFileSystem, IDisposable
    {
        bool IsConnected();
        void Connect();
        void Disconnect();

        void SetWorkingDirectory(string path);
        void SetRootAsWorkingDirectory();

        void UploadFile(string localFilePath, string remoteFilePath);
        void DownloadFile(string localFilePath, string remoteFilePath);
        Stream OpenRead(string remoteFilePath, FtpDataType type);

        string ServerDetails();
    }
}
