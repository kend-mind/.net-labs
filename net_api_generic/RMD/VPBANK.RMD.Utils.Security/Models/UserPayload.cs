namespace VPBANK.RMD.Utils.Security.Models
{
    public class UserPayload
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DefaultRole { get; set; }
        public string DataRole { get; set; }
        public string FunctionRole { get; set; }
        public string ComponentRole { get; set; }
        public string Status { get; set; }
    }

    public class UserClaimKey
    {
        public const string ID = "id";
        public const string USERNAME = "username";
        public const string EMAIL = "email";
        public const string DEFAULT_ROLE = "defaultRole";
        public const string DATA_ROLE = "dataRole";
        public const string FUNCTION_ROLE = "functionRole";
        public const string COMPONENT_ROLE = "componentRole";
        public const string STATUS = "status";
    }
}
