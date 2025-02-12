using Nest;
using System;
using System.Collections.Generic;
using VPBANK.RMD.Utils.AuditLog.Models;

namespace VPBANK.RMD.Utils.Notification
{
    public class Notification
    {
        [Keyword]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime recvTime { get; set; }
        public string messageType { get; set; }
        public string type { get; set; }
        public string routingKey { get; set; }
        public string[] receiver { get; set; }
        public string payload { get; set; }
        public IDictionary<string, object> extractInfo { get; set; } = new Dictionary<string, object>();
    }

    public class NotificationHistory : IElasticLog
    {
        [Keyword]
        public string id { get; set; } = Guid.NewGuid().ToString();

        public DateTime? recvTime { get; set; }
        /// <summary>
        /// Notification types
        /// </summary>
        public string messageType { get; set; }
        /// <summary>
        /// Action types
        /// </summary>
        public string type { get; set; }
        public string receiver { get; set; }
        public string payload { get; set; }
        public bool? seen { get; set; }
        public IDictionary<string, object> extractInfo { get; set; } = new Dictionary<string, object>();
    }
}
