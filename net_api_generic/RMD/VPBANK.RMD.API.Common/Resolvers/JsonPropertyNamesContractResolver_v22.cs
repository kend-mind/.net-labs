using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text;
using VPBANK.RMD.Utils.Common.Shared;
using static VPBANK.RMD.Utils.Common.Enums;

namespace VPBANK.RMD.API.Common.Resolvers
{
    /// <summary>
    /// Json custom convert property names
    /// </summary>
    public class JsonPropertyNamesContractResolver_v22 : DefaultContractResolver
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public JsonPropertyNamesContractResolver_v22(bool shareCache = false) : base()
        {
            ResolverCase = ResolverCaseEnums.Camelize;
            KeepUnderscores = true;
        }

        public ResolverCaseEnums ResolverCase { get; set; }
        public bool KeepUnderscores { get; set; }

        protected override string ResolvePropertyName(string propertyName)
        {
            return ChangeResolverCase(propertyName);
        }

        // Change to case
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

        // Json.NET implementation for reference
        private static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
                return s;
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; ++i)
            {
                if (i == 0 || i + 1 >= s.Length || char.IsUpper(s[i + 1]))
                    sb.Append(char.ToLower(s[i], Culture));
                else
                {
                    sb.Append(s.Substring(i));
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
