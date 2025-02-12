using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VPBANK.RMD.Repositories.Auth.Implements;
using System.Linq;
using System;
using Serilog;
using VPBANK.RMD.API.Common.Middlewares;
using System.Net;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.Controllers.PhoenixConf.Tech
{
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        readonly UserRepository _repository;

        public AuthController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public virtual IActionResult FindAll()
        {
            try
            {
                return Ok(_repository.Queryable().AsEnumerable().ToList());
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