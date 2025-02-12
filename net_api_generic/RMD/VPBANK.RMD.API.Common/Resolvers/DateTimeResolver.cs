using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VPBANK.RMD.API.Common.Resolvers
{
    public class DateTimeResolver : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            var result = DateTime.Parse(reader.GetString());
            return result;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
