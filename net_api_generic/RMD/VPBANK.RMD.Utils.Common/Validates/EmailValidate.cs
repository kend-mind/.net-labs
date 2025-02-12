using System.Text.RegularExpressions;

namespace VPBANK.RMD.Utils.Common.Validates
{
    public static class EmailValidate
    {
        public static bool IsValid(string email)
        {
            var regexPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            return Regex.IsMatch(email, regexPattern, RegexOptions.IgnoreCase);
        }
    }
}
