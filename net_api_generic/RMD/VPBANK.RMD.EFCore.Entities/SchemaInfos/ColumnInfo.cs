using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VPBANK.RMD.EFCore.Entities.SchemaInfos
{
    public class ColumnInfo : BaseQueryResult
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

        [JsonPropertyName("field")]
        [JsonProperty("field", NullValueHandling = NullValueHandling.Include)]
        public string COLUMN_NAME { get; set; }

        [JsonPropertyName("order")]
        [JsonProperty("order", NullValueHandling = NullValueHandling.Include)]
        public int ORDINAL_POSITION { get; set; }

        [JsonPropertyName("columnDefault")]
        [JsonProperty("columnDefault", NullValueHandling = NullValueHandling.Include)]
        public string COLUMN_DEFAULT { get; set; }

        [JsonPropertyName("nullable")]
        [JsonProperty("nullable", NullValueHandling = NullValueHandling.Include)]
        public string IS_NULLABLE { get; set; }

        [JsonPropertyName("dataType")]
        [JsonProperty("dataType", NullValueHandling = NullValueHandling.Include)]
        public string DATA_TYPE { get; set; }

        [JsonPropertyName("maxLength")]
        [JsonProperty("maxLength", NullValueHandling = NullValueHandling.Include)]
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }

        [JsonPropertyName("precision")]
        [JsonProperty("precision", NullValueHandling = NullValueHandling.Include)]
        public byte? NUMERIC_PRECISION { get; set; }

        [JsonPropertyName("scale")]
        [JsonProperty("scale", NullValueHandling = NullValueHandling.Include)]
        public int? NUMERIC_SCALE { get; set; }

        [NotMapped]
        [JsonPropertyName("object")]
        [JsonProperty("object", NullValueHandling = NullValueHandling.Include)]
        public string OBJECT_NAME
        {
            get { return TABLE_NAME; }
        }
    }
}
