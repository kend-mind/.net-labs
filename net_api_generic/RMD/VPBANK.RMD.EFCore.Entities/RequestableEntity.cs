using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VPBANK.RMD.EFCore.Entities
{
    public interface IRequestableEntity
    {
        public string ReqActionType { get; set; }
        public string ReqObjectType { get; set; }
        public string ReqDetailId { get; set; }
        public string SyncId { get; set; }
    }

    public abstract class RequestableEntity : TrackableEntity, IRequestableEntity
    {
        /// <summary>
        /// Insert, Update, Delete
        /// </summary>
        [NotMapped]
        [JsonPropertyName("$ACTION")]
        [JsonProperty("$ACTION", NullValueHandling = NullValueHandling.Ignore)]
        public string ReqActionType { get; set; }

        /// <summary>
        /// New, Old
        /// </summary>
        [NotMapped]
        [JsonPropertyName("$OBJECT_TYPE")]
        [JsonProperty("$OBJECT_TYPE", NullValueHandling = NullValueHandling.Ignore)]
        public string ReqObjectType { get; set; }

        /// <summary>
        /// 1, 2, 3, 4 auto imcrement
        /// </summary>
        [NotMapped]
        [JsonPropertyName("$REQUEST_DETAIL_ID")]
        [JsonProperty("$REQUEST_DETAIL_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string ReqDetailId { get; set; }

        /// <summary>
        /// Guid, audit record
        /// </summary>
        [NotMapped]
        [JsonPropertyName("$SYNC_ID")]
        [JsonProperty("$SYNC_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string SyncId { get; set; }
    }
}
