using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace VPBANK.RMD.EFCore.Entities.SchemaInfos
{
    public class TableInfo : BaseQueryResult
    {
        [JsonPropertyName("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Include)]
        public string TABLE_CATALOG { get; set; }
        [JsonPropertyName("tableSchema")]
        [JsonProperty("tableSchema", NullValueHandling = NullValueHandling.Include)]
        public string TABLE_SCHEMA { get; set; }
        [JsonPropertyName("tableName")]
        [JsonProperty("tableName", NullValueHandling = NullValueHandling.Include)]
        public string TABLE_NAME { get; set; }
        [JsonPropertyName("tableType")]
        [JsonProperty("tableType", NullValueHandling = NullValueHandling.Include)]
        public string TABLE_TYPE { get; set; }
    }
}
