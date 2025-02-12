using FluentFTP;
using Renci.SshNet;
using System.IO;
using VPBANK.RMD.Utils.Common.Remote.FTP;

namespace VPBANK.RMD.Utils.Common.Remote.Contexts
{
    public abstract class SftpContext : IRemoteFileSystemContext
    {
        protected SftpClient _sftpClient { get; set; }

        public bool IsConnected()
        {
            return _sftpClient.IsConnected;
        }

        public void Connect()
        {
            _sftpClient.Connect();
        }

        public void Disconnect()
        {
            _sftpClient.Disconnect();
        }

        public void Dispose()
        {
            if (_sftpClient != null)
                _sftpClient.Dispose();
        }

        public bool FileExists(string filePath)
        {
            return _sftpClient.Exists(filePath);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return _sftpClient.Exists(directoryPath);
        }

        public void CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!DirectoryExists(directoryPath))
                _sftpClient.CreateDirectory(directoryPath);
        }

        public void DeleteFileIfExists(string filePath)
        {
            if (!FileExists(filePath))
                _sftpClient.DeleteFile(filePath);
        }

        public void SetWorkingDirectory(string directoryPath)
        {
            _sftpClient.ChangeDirectory(directoryPath);
        }

        public void SetRootAsWorkingDirectory()
        {
            SetWorkingDirectory(string.Empty);
        }

        public void UploadFile(string localFilePath, string remoteFilePath)
        {
            using var fileStream = new FileStream(localFilePath, FileMode.Open);
            _sftpClient.UploadFile(fileStream, remoteFilePath);
        }


        public void DownloadFile(string localFilePath, string remoteFilePath)
        {
            using Stream fileStream = File.Create(localFilePath);
            _sftpClient.DownloadFile(remoteFilePath, fileStream);
        }

        public Stream OpenRead(string remoteFilePath, FtpDataType type)
        {
            return _sftpClient.OpenRead(remoteFilePath);
        }

        public abstract string ServerDetails();
    }
}
