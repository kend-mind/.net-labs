using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.Auth.Controllers
{
    [AllowAnonymous]
    [Route(template: "")]
    public class InfoController : ControllerBase
    {
        protected IMemoryCache _memoryCache;
        protected readonly IConfiguration _configuration;
        protected IWebHostEnvironment _env;
        protected readonly IAppSettingsReader _appSettings;
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IMapper _mapper;

        public InfoController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _env = env;
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual IActionResult Version()
        {
            try
            {
                return Ok(new { version = SwaggerSetting.GetSwaggerInfoSection(_configuration).Version });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }
    }
}
