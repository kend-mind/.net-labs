namespace VPBANK.RMD.Utils.Common
{
    public class ApiKeys
    {
        public const string X_AUTH_TOKEN = "X-AUTH-TOKEN";
        public const string X_API_VERSION = "X-API-VERSION";
        public const string VBP_RMD_Policy = "VBP_RMD_Policy";

        public const string X_FORWARDER_FOR = "X-Forwarded-For";
        public const string REMOTE_ADDR = "REMOTE_ADDR";

        public const string HEADER_ACCESS_CONTROL_EXPOSE = "Access-Control-Expose-Headers";
        public const string HEADER_ACCESS_CONTROL_EXPOSE_VAL = "Content-Disposition";

        public const string PROBLEM_CONTENT_TYPE = "application/problem+json";

        public const string API1 = "API1";
        public const string API2 = "API2";

        public const string API_VERSION = "1.0";
        public const string API_AUTHOR_HEADER = "author";
        public const string API_AUTHOR = "xuandt10@vpbank.com.vn";

        public const string ROUTE_TEMPLATE = "api/vbp-rmd/v1.0/[controller]/[action]";
        public const string ROUTE_TEMPLATE_VERSION = "api/vbp-rmd/v{version:apiVersion}/[controller]/[action]";
    }
}
