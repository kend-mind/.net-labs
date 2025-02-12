using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace VPBANK.RMD.Utils.Common.Extensions
{
    public static class ObjectSerializeExtentions
    {
        private static JsonSerializerSettings JSetting = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatString = DefFormats.DATETIME_FORMAT_Z
        };

        public static byte[] Serialize<T>(this T obj) where T : class
        {
            if (obj == null) return new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static T DeSerialize<T>(this byte[] arr) where T : class
        {
            if (arr == null || arr.Length == 0) return default;

            T obj = default(T);
            using (MemoryStream stream = new MemoryStream(arr))
            {
                IFormatter formatter = new BinaryFormatter();
                stream.Write(arr, 0, arr.Length);
                stream.Seek(0, SeekOrigin.Begin);
                obj = (T)formatter.Deserialize(stream);
            }
            return obj;
        }

        public static string DeSerializeText(this byte[] arrBytes)
        {
            return Encoding.Default.GetString(arrBytes);
        }

        public static byte[] JsonSerialize(this object obj)
        {
            if (obj == null) return null;

            var json = JsonConvert.SerializeObject(obj, JSetting);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T JsonDeSerialize<T>(this byte[] arrBytes)
        {
            var json = Encoding.UTF8.GetString(arrBytes);
            return JsonConvert.DeserializeObject<T>(json, JSetting);
        }
    }
}
