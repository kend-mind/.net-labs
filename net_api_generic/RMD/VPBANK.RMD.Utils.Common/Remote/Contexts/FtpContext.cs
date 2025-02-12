using FluentFTP;
using System.IO;
using VPBANK.RMD.Utils.Common.Remote.FTP;

namespace VPBANK.RMD.Utils.Common.Remote.Contexts
{
    public abstract class FtpContext : IRemoteFileSystemContext
    {
        protected IFtpClient _ftpClient { get; set; }

        public bool IsConnected()
        {
            return _ftpClient.IsConnected;
        }

        public void Connect()
        {
            _ftpClient.Connect();
        }

        public void Disconnect()
        {
            _ftpClient.Disconnect();
        }

        public void Dispose()
        {
            if (_ftpClient != null && !_ftpClient.IsDisposed)
                _ftpClient.Dispose();
        }

        public bool FileExists(string filePath)
        {
            return _ftpClient.FileExists(filePath);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return _ftpClient.DirectoryExists(directoryPath);
        }

        public void CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!DirectoryExists(directoryPath))
                _ftpClient.CreateDirectory(directoryPath);
        }

        public void DeleteFileIfExists(string filePath)
        {
            if (!FileExists(filePath))
                _ftpClient.DeleteFile(filePath);
        }

        public void SetWorkingDirectory(string directoryPath)
        {
            _ftpClient.SetWorkingDirectory(directoryPath);
        }

        public void SetRootAsWorkingDirectory()
        {
            SetWorkingDirectory(string.Empty);
        }

        public void UploadFile(string localFilePath, string remoteFilePath)
        {
            _ftpClient.UploadFile(localFilePath, remoteFilePath);
        }

        public void DownloadFile(string localFilePath, string remoteFilePath)
        {
            _ftpClient.DownloadFile(localFilePath, remoteFilePath, FtpLocalExists.Overwrite);
        }

        public Stream OpenRead(string remoteFilePath, FtpDataType type)
        {
            return _ftpClient.OpenRead(remoteFilePath, type);
        }

        public abstract string ServerDetails();
    }
}
