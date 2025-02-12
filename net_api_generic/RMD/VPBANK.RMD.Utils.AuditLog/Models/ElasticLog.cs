using Nest;
using System;

namespace VPBANK.RMD.Utils.AuditLog.Models
{
    public class ElasticLog : IElasticLog
    {
        public ElasticLog()
        {
            timestamp = DateTime.Now;
        }

        // default elastic search
        [Keyword]
        public DateTime timestamp { get; set; }
    }
}
