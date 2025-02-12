using System.Collections.Generic;

namespace VPBANK.RMD.API.Settings
{
    public class AppSettings
    {
        public string ApiUri { get; set; }
        public string ApiUri_Jav { get; set; }
        public ApiSwaggerInfoSection ApiSwaggerInfo { get; set; }
        public ConnectionStringsSection ConnectionStrings { get; set; }
        public JwtSection Jwt { get; set; }
        public RabbitMqConf RabbitMq { get; set; }
        public ElasticAuditSection ElasticAudit { get; set; }
        public FtpInfoSection FtpInfo { get; set; }
    }

    #region ApiSwaggerInfo

    public class ApiSwaggerInfoSection
    {
        public string Version { get; set; }
        public string ApiDocument { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public ApiSwaggerContact Contact { get; set; }
        public ApiSwaggerLicense License { get; set; }
        public ApiSwaggerEndpoint Endpoint { get; set; }
    }

    public class ApiSwaggerContact
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }

    public class ApiSwaggerLicense
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class ApiSwaggerEndpoint
    {
        public string RoutePrefix { get; set; }
        public string EndpointUrl { get; set; }
        public string EndpointSelectDrop { get; set; }
    }

    #endregion

    #region ConnectionStrings

    public class ConnectionStringsSection
    {
        public string AuthConnection { get; set; }
        public string PhoenixConfConnection { get; set; }
        public string PhoenixDataConnection { get; set; }
        public string CollectionConnection { get; set; }
        public string IFRS9ConfConnection { get; set; }
        public string IFRS9DataConnection { get; set; }
    }

    #endregion

    #region JWT

    public class JwtSection
    {
        public string SecretKey { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public List<string> Issuers { get; set; }
        public string Audience { get; set; }
        public List<string> Audiences { get; set; }
        public int Lifetime { get; set; }
        public int ExpirationInMins { get; set; }
        public float NotBeforeInSeconds { get; set; }
        public string JwtTokenCookie { get; set; }
    }

    #endregion

    #region RabbitMq

    public class RabbitMqConf
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; } = 5672;
        public string VirtualHostUrl { get; set; } = "/";
        public string APIUrl { get; set; }
        public string APIKey { get; set; }
        public string APISecretKey { get; set; }
        public string NotiExchange { get; set; }
        public string NotiQueue { get; set; }
        public string RouterKey { get; set; }
    }

    public class RabbitMqSection : RabbitMqConf
    {
    }

    #endregion

    #region Email

    public class EmailSection
    {

    }

    #endregion

    #region Elastic-search

    public class ElasticAuditSection
    {
        public string Uri { get; set; }
        public bool FixAlias { get; set; }
        public string NotiAlias { get; set; }
        public string AuditAlias { get; set; }
        public bool IndexPerYear { get; set; }
        public int AmountOfPreviousIndicesUsedInAlias { get; set; }
    }

    public class FtpInfoSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FolderReq { get; set; }
    }

    #endregion
}
