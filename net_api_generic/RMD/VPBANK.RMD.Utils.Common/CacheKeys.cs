namespace VPBANK.RMD.Utils.Common
{
    public static class CacheKeys
    {
        public static readonly string ApiSwaggerInfo = "_Api_Swagger_Info";
        public static readonly string CancelTokenSource = "_Cancel_Token_Source";
        public static readonly string CallbackEntry = "_Callback_Entry";
        public static readonly string User = "_User";
        public static readonly string UserPayload = "_User_Payload";

        public static readonly string SqlExceptionCache = "_{0}_Approve_SqlExceptionCache";
    }

    public enum CacheType
    {
        Memory,
        Redis
    }

    public class CacheConfiguration
    {
        public int AbsoluteExpirationInHours { get; set; }
        public int SlidingExpirationInMinutes { get; set; }

        public void GetSettings(int absoluteExpirationInHours, int slidingExpirationInMinutes)
        {
            AbsoluteExpirationInHours = absoluteExpirationInHours;
            SlidingExpirationInMinutes = slidingExpirationInMinutes;
        }
    }
}
