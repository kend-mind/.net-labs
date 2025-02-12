using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.Settings.Sections
{
    public static class JsonSetting
    {
        /// <summary>
        /// Config datetime format
        /// </summary>
        public static JsonSerializerSettings ConfigFormat => new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatString = DefFormats.DATETIME_FORMAT_Z
        };
    }
}
