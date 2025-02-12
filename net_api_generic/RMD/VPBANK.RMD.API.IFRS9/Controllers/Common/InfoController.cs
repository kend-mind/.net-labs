using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VPBANK.RMD.API.Settings;
using Microsoft.AspNetCore.Authorization;
using VPBANK.RMD.API.Common.Middlewares;
using Serilog;
using System.Net;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.IFRS9.Controllers.Common
{
    [AllowAnonymous]
    [Route(template: "")]
    public class InfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InfoController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                Log.Error(ex.InnerException?.Message);
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
