using Nest;
using System;

namespace VPBANK.RMD.Utils.AuditLog.Models
{
    public class AuditLog : IElasticLog
    {
        public AuditLog()
        {
            timestamp = DateTime.Now;
        }

        // default elastic search -> @
        public DateTime timestamp { get; set; }


        [Keyword]
        public string uuid { get; set; }
        public string type { get; set; }
        public string action { get; set; }
        public string classname { get; set; }
        public DateTime client_date { get; set; }
        public string endpoint { get; set; }
        public string host { get; set; }
        public string ip { get; set; }
        public string message { get; set; }
        public string payload { get; set; }
        public string user { get; set; }
    }
}
