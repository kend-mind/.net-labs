using System;

namespace VPBANK.RMD.Utils.Common.Datas
{
    //[Serializable]
    public class FileUploadReq
    {
        public string DocumentType { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public string HostInfo { get; set; }
        public string FileType { get; set; }
        public string TargetServer { get; set; }
    }

    //[Serializable]
    public class FileUploadInfo
    {
        public string filename { get; set; }
        public string path { get; set; }
        public string hostInfo { get; set; }
        public string fileType { get; set; }
        public string targetServer { get; set; }
        public long size { get; set; } = 0;
        public DateTime createDt { get; set; } = DateTime.Now;
        public DateTime updateDt { get; set; } = DateTime.Now;
    }
}
