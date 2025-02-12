using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using Serilog;
using System.Net;
using VPBANK.RMD.Utils.AuditLog;
using VPBANK.RMD.Utils.AuditLog.Models;
using VPBANK.RMD.API.Common.Helpers.Requests;
using VPBANK.RMD.Utils.Common.Shared;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.API.Common.Extensions;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Utils.Security.Models;
using VPBANK.RMD.Utils.Common.Helpers;
using VPBANK.RMD.API.Logs;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.API.Common.Filters
{
    /// <summary>
    /// 1	Global OnActionExecuting
    /// 2	Controller OnActionExecuting
    /// 3	Method OnActionExecuting
    /// 4	Method OnActionExecuted
    /// 5	Controller OnActionExecuted
    /// 6	Global OnActionExecuted
    /// </summary>
    public class ActionFilterHandler : IActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly RequestHandler _requestHandler;
        private readonly ILoggerManager _logger;
        private readonly IAuditElasticProvider<AuditLog> _auditElasticProvider;

        public ActionFilterHandler(IConfiguration configuration,
            RequestHandler requestHandler,
            ILoggerManager logger,
            IAuditElasticProvider<AuditLog> auditElasticProvider)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
            _logger = logger;
            _auditElasticProvider = auditElasticProvider;
        }

        /// <summary>
        /// do something before the action executes
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == API_METHODS.GET)
            {
                context.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                context.HttpContext.Response.Headers.Add("Expires", "-1");
            }

            // logs endpoint
            Log.Information($"Action Executing -> endpoint: {_requestHandler.GetHostFull()}");

            // logs parameters
            if (context.ActionArguments != null && context.ActionArguments.Count > 0)
                Log.Information($"Action Executing -> params: {JsonConvert.SerializeObject(context.ActionArguments, Formatting.None)}");
            else
                Log.Information("Action Executing -> params: none");
        }

        /// <summary>
        /// do something after the action executes
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            #region Logs activities to elk

            try
            {
                var user = SessionHelper.Get<UserPayload>(_requestHandler.GetSessionRequest(), SESSION_KEYS.USER_PAYLOAD);
                if (user == null)
                    throw new HttpErrorException(HttpStatusCode.Unauthorized, nameof(HttpStatusCode.Unauthorized), ErrorMessages.SE402);

                var endPoint = _requestHandler.GetEndpoint();
                var action = string.Empty;
                if (endPoint.ToLower().EndsWith(@"/req"))
                    action = ActionTypes.REQUEST.ToDescription();
                else if (endPoint.ToLower().Contains(@"/apr/") || endPoint.Contains(@"/request/approve/") || endPoint.ToLower().Contains(@"/approve/"))
                    action = ActionTypes.APPROVAL.ToDescription();
                else if (endPoint.ToLower().Contains(@"/cancel/"))
                    action = ActionTypes.CANCEL.ToDescription();
                else if (endPoint.ToLower().Contains(@"/reject/"))
                    action = ActionTypes.REJECT.ToDescription();
                else if (endPoint.ToLower().Contains(@"/create") || endPoint.ToLower().Contains(@"/batch-create"))
                    action = ActionTypes.INSERT.ToDescription();
                else if (endPoint.ToLower().Contains(@"/update/") || endPoint.ToLower().EndsWith(@"/batch-update"))
                    action = ActionTypes.UPDATE.ToDescription();
                else if (endPoint.ToLower().Contains(@"/delete/") || endPoint.ToLower().EndsWith(@"/batch-delete"))
                    action = ActionTypes.DELETE.ToDescription();
                else if (endPoint.ToLower().EndsWith(@"/query") || endPoint.ToLower().Contains(@"/find-by-id/"))
                    action = ActionTypes.QUERY.ToDescription();
                else if (endPoint.ToLower().EndsWith(@"/send-req") || endPoint.ToLower().Contains(@"/upd-req/") || endPoint.ToLower().Contains(@"/apr-req/"))
                    action = ActionTypes.UPLOAD.ToDescription();
                else
                    action = ActionTypes.QUERY.ToDescription();

                if (!string.IsNullOrEmpty(action))
                {
                    // log to file
                    var msg = $"{user.Username} {action} at {DateTime.Now.ToString(DefFormats.DATETIME_FORMAT_ICT)} [{Guid.NewGuid()} endpoint=\"{_requestHandler.GetHostFull()}\" ip={_requestHandler.GetRequestIP()} obj=\"{"NULL"}\" payload=\"{JsonConvert.SerializeObject(context.Result, Formatting.None)}\"]";
                    _logger.Info(msg);
                    // log to elk
                    //var auditLog = new AuditLog
                    //{
                    //    uuid = uuid,
                    //    classname = "NUL",
                    //    action = action,
                    //    client_date = DateTime.Now,
                    //    user = user.Username,
                    //    endpoint = _requestHandler.GetHostFull(),
                    //    ip = _requestHandler.GetRequestIP(),
                    //    host = _requestHandler.GetHostBase(),
                    //    message = msg,
                    //    payload = JsonConvert.SerializeObject(context.Result, Formatting.None),
                    //    type = ElasticSearchTypes.IPhoenixService.ToDescription()
                    //};
                    //auditLog.message = msg;
                    //_auditElasticProvider.AddLog(auditLog);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error logs audit activities: {ex.Message}");
                Log.Error($"Error logs audit activities: {ex.StackTrace}");
            }

            #endregion
        }
    }
}
