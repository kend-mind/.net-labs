using System.Globalization;
using System.Text;
using System.Text.Json;
using VPBANK.RMD.Utils.Common.Shared;
using static VPBANK.RMD.Utils.Common.Enums;

namespace VPBANK.RMD.API.Common.Resolvers
{
    /// <summary>
    /// Json custom convert property names
    /// </summary>
    public class JsonPropertyNamesContractResolver_v31 : JsonNamingPolicy
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public JsonPropertyNamesContractResolver_v31(bool shareCache = false) : base()
        {
            ResolverCase = ResolverCaseEnums.Camelize;
            KeepUnderscores = true;
        }

        public ResolverCaseEnums ResolverCase { get; set; }
        public bool KeepUnderscores { get; set; }

        public override string ConvertName(string propertyName)
        {
            return ChangeResolverCase(propertyName);
        }

        /// <summary>
        /// Convert to property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string ChangeResolverCase(string propertyName)
        {
            var sb = new StringBuilder(propertyName.Length);

            bool isNextUpper = ResolverCase == ResolverCaseEnums.Pascalize, isPrevLower = false;
            foreach (var c in propertyName)
            {
                if (c == SpecificCharacteristics.UNDERSCORE)
                {
                    if (KeepUnderscores)
                        sb.Append(c);
                    isNextUpper = true;
                }
                else
                {
                    sb.Append(isNextUpper ? char.ToUpper(c, Culture) : isPrevLower ? c : char.ToLower(c, Culture));
                    isNextUpper = false;
                    isPrevLower = char.IsLower(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Json.NET implementation for reference
        /// Convert to collName
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string ToCamelCase(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !char.IsUpper(propertyName[0]))
                return propertyName;
            var sb = new StringBuilder();
            for (int i = 0; i < propertyName.Length; ++i)
            {
                if (i == 0 || i + 1 >= propertyName.Length || char.IsUpper(propertyName[i + 1]))
                    sb.Append(char.ToLower(propertyName[i], Culture));
                else
                {
                    sb.Append(propertyName.Substring(i));
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
