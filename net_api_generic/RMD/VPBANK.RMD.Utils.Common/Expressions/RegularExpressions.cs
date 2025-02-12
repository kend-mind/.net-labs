namespace VPBANK.RMD.Utils.Common.Expressions
{
    public class RegularExpressions
    {
        public const string EMAIL = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string PHONE_NUMBER = @"^[0-9]{1,}";
    }
}
