using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.Common.Helpers.Requests
{
    public class RequestHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext => _httpContextAccessor.HttpContext;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public RequestHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext;
        }

        public ISession GetSessionRequest()
        {
            return _session;
        }

        public void GetAboutRequest()
        {
            var message = "The HttpContextAccessor seems to be working!!";
            _httpContextAccessor.HttpContext.Session.SetString(nameof(message), message);
        }

        public string GetHostBase()
        {
            return $"{_httpContext.Request.Host.Host}:{_httpContext.Request.Host.Port}";
        }

        public string GetHostFull()
        {
            return $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.Path}{_httpContext.Request.QueryString}";
        }

        public string GetEndpoint()
        {
            var routeEndpoint = (RouteEndpoint)_httpContext?.GetEndpoint();
            return routeEndpoint?.RoutePattern?.RawText;
        }

        public string GetRemoteIpAddress(bool mapToIpv4)
        {
            string remoteIpAddress = mapToIpv4
                ? _httpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString()
                : _httpContext?.Connection?.RemoteIpAddress?.ToString();

            if (_httpContext.Request.Headers.ContainsKey(ApiKeys.X_FORWARDER_FOR))
                remoteIpAddress = _httpContext.Request.Headers[ApiKeys.X_FORWARDER_FOR];

            return remoteIpAddress;
        }

        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>(ApiKeys.X_FORWARDER_FOR);

                if (string.IsNullOrWhiteSpace(csvList))
                    ip = string.Empty;
                else
                {
                    var ips = csvList.TrimEnd(',').Split(',').AsEnumerable<string>().Select(s => s.Trim()).ToList();
                    ip = ips.FirstOrDefault();
                }
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>(ApiKeys.REMOTE_ADDR);

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrEmpty(ip) || string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                // writes out as Csv when there are multiple.
                string rawValues = values.ToString();

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}
