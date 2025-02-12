using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using AutoMapper;
using System;
using Serilog;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.AuditLog;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.Utils.AuditLog.Models;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Controllers;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.API.IFRS9.Controllers.Common
{
    [Authorize]
    public class AuditLogController : BaseController
    {
        private readonly IAuditElasticProvider<AuditLog> _elasticProvider;

        public AuditLogController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            IRabbitMqPublisher rabbitManager,
            ISubscriberInfoRepository subscriberRepository,

            IAuditElasticProvider<AuditLog> elasticProvider) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository)
        {
            _elasticProvider = elasticProvider;
        }

        /// <summary>
        /// Add new audit logs
        /// </summary>
        /// <param name="elasticLog"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ActionResult<IElasticLog>> Create([FromBody] AuditLog elasticLog)
        {
            try
            {
                // check object valid
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _elasticProvider.AddLogAsync(elasticLog);

                // results
                return CreatedAtAction(nameof(Create), new { id = elasticLog.timestamp }, elasticLog);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
                // out method
            }
        }

        /// <summary>
        /// Query audit logs
        /// </summary>
        /// <param name="elasticSearchPaging"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ActionResult<IElasticLog>> Query([FromBody] ElasticSearchPaging elasticSearchPaging)
        {
            try
            {
                // check object valid
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var results = await _elasticProvider.QueryAuditLogsAsync(elasticSearchPaging);

                // results
                return Ok(results);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
                // out method
            }
        }
    }
}