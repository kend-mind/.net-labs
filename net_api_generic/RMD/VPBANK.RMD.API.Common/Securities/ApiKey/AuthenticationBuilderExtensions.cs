using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Utils.Security.SRAJose;
using VPBANK.RMD.API.Settings.Sections;
using VPBANK.RMD.API.Common.Helpers.Requests;
using VPBANK.RMD.API.Common.Extensions;

namespace VPBANK.RMD.API.Common.Securities.ApiKey
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySraJose(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = ApiKeys.PROBLEM_CONTENT_TYPE;
        private readonly IConfiguration _configuration;
        private readonly RequestHandler _requestHandler;
        private readonly IUserRepository _userRepository;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration,
            RequestHandler requestHandler,
            IUserRepository userRepository) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
            _userRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // check API KEY
            if (!Request.Headers.TryGetValue(ApiKeys.X_AUTH_TOKEN, out var apiKeyHeaderValues))
            {
                Logger.LogWarning($"Invalid API KEY [{ApiKeys.X_AUTH_TOKEN}] provided.");
                return AuthenticateResult.NoResult();
            }

            // check TOKEN
            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                Logger.LogInformation($"Token: {providedApiKey}");
                Logger.LogError($"Invalid TOKEN of API KEY [{ApiKeys.X_AUTH_TOKEN}] provided.");
                return AuthenticateResult.NoResult();
            }
            var validToken = string.IsNullOrEmpty(providedApiKey) ? "Invalid" : "Valid";
            Logger.LogInformation($"Token: {validToken}");

            // check PUBLIC_KEY
            var jwtConfig = JWTSetting.GetJwtSection(_configuration);
            var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), jwtConfig.PublicKey);
            var publicKey = File.ReadAllText(publicKeyPath);

            // check payload from token
            var payload = TokenConsumer.Consume(providedApiKey, publicKey);
            if (payload != null && !string.IsNullOrEmpty(payload.Username))
            {
                //var user = await _userRepository.FindByUserNameAndIsDeletedAsync(payload.Username, 0.ToString());
                //if (user != null)
                //{
                //    // save to session
                //    SessionHelper.Set(_requestHandler.GetSessionRequest(), SESSION_KEYS.USER_PAYLOAD, payload);

                //    // build claim and save to Identity
                //    var claims = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.Name, payload.Username)
                //    };
                //    var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                //    var identities = new List<ClaimsIdentity> { identity };
                //    var principal = new ClaimsPrincipal(identities);
                //    var ticket = new AuthenticationTicket(principal, Options.Scheme);

                //    return AuthenticateResult.Success(ticket);
                //}
            }

            return AuthenticateResult.Fail("Invalid API KEY provided.");
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new UnauthorizedProblemDetails();
            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new ForbiddenProblemDetails();
            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = ApiKeys.X_AUTH_TOKEN;
        public string Scheme => DefaultScheme;
        public string AuthenticationType => DefaultScheme;
    }

    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions Options => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };
    }
}
