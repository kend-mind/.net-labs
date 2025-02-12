using VPBANK.RMD.Utils.Common.Shared;
using System;
using System.Text;

namespace VPBANK.RMD.Utils.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCaseWithUnderscore(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            var isFirstChar = true;

            var sb = new StringBuilder(str.Length);
            foreach (var c in str)
            {
                if (c == str[0] && isFirstChar)
                {
                    sb.Append(c.ToString().ToUpper());
                    isFirstChar = false;
                    continue;
                }

                if (Constants.CHARS_ALL.Contains(c.ToString(), StringComparison.CurrentCulture))
                    sb.Append(SpecificCharacteristics.UNDERSCORE);
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ToCamelCase(this string str)
        {
            bool hasValue = !string.IsNullOrEmpty(str);

            // doesn't have a value or already a camelCased word
            if (!hasValue || (hasValue && char.IsLower(str[0])))
            {
                return str;
            }

            string finalStr = "";

            int len = str.Length;
            int idx = 0;

            char nextChar = str[idx];

            while (char.IsUpper(nextChar))
            {
                finalStr += char.ToLowerInvariant(nextChar);

                if (len - 1 == idx)
                {
                    // end of string
                    break;
                }

                nextChar = str[++idx];
            }

            // if not end of string 
            if (idx != len - 1)
            {
                finalStr += str.Substring(idx);
            }

            return finalStr;
        }
    }
}
