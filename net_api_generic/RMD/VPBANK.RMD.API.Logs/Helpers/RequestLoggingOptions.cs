using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;

namespace VPBANK.RMD.API.Logs.Helpers
{
    public class RequestLoggingOptions
    {
        public string MessageTemplate { get; set; }
        public Func<HttpContext, double, Exception, LogEventLevel> GetLevel { get; set; }
        public Action<IDiagnosticContext, HttpContext> EnrichDiagnosticContext { get; set; }
    }
}
