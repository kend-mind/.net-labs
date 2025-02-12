using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using VPBANK.RMD.Utils.AuditLog;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.Utils.AuditLog.Models;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Controllers;
using AutoMapper;
using VPBANK.RMD.Utils.Common.Helpers.Paging;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Utils.Notification;

namespace VPBANK.RMD.API.IFRS9.Controllers.Common
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private INotiElasticProvider<NotificationHistory> _notiElasticProvider;

        public NotificationController(IMemoryCache memoryCache,
           IConfiguration configuration,
           IWebHostEnvironment env,
           IAppSettingsReader appSettings,
           IHttpClientFactory httpClientFactory,
           IMapper mapper,
           IRabbitMqPublisher rabbitManager,
           ISubscriberInfoRepository subscriberRepository,

           INotiElasticProvider<NotificationHistory> notiElasticProvider) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository)
        {
            _notiElasticProvider = notiElasticProvider;
        }

        /// <summary>
        /// Query find notification from elasticsearch, search by session user logined, no filters, has paginated
        /// </summary>
        /// <param name="paginatedParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Query([FromBody] PaginatedInputModel paginatedParams)
        {
            if (paginatedParams == null)
                return null;

            var param = new ElasticSearchPaging
            {
                From = paginatedParams.PageIndex,
                Size = paginatedParams.PageSize,
                Filters = GetUserPayload().Username
            };

            try
            {
                return Ok(await GetHistories(paginatedParams, param));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("invalid_alias_name_exception", StringComparison.CurrentCultureIgnoreCase))
                    return Ok(await GetHistories(paginatedParams, param));
                return null;
            }
        }

        private async Task<PaginatedContentResults<object>> GetHistories(PaginatedInputModel paginatedParams, ElasticSearchPaging param)
        {
            try
            {
                var data = await _notiElasticProvider.QueryAuditLogsAsync(param);
                param.From = 0;
                param.Size = 10000;

                var total = await _notiElasticProvider.CountAsync(param);
                dynamic result = await SqlQueryDataAsync(Convert.ToInt32(total), data, paginatedParams);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}