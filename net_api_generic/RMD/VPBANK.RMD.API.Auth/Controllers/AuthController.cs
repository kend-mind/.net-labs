using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using AutoMapper;
using Serilog;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Extensions;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Security.SRAJose;
using VPBANK.RMD.API.Settings.Sections;
using VPBANK.RMD.Utils.Security.Models;
using VPBANK.RMD.API.Common.Controllers;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Services.Auth.DataContracts;

namespace VPBANK.RMD.API.Auth.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IViewUserRoleRepository _viewUserRoleRepository;

        public AuthController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            IRabbitMqPublisher rabbitManager,
            ISubscriberInfoRepository subscriberRepository,

            IUserRepository userRepository,
            IViewUserRoleRepository viewUserRoleRepository) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository)
        {
            this._userRepository = userRepository;
            this._viewUserRoleRepository = viewUserRoleRepository;
        }

        #region Jose SRA

        /// <summary>
        /// Build token when login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] UserAuthDto model)
        {
            try
            {
                // check valid
                if (!ModelState.IsValid || model == null || string.IsNullOrEmpty(model.Username))
                    throw new HttpErrorException(HttpStatusCode.BadGateway, nameof(HttpStatusCode.BadGateway), ErrorMessages.SE003);

                var user = await _userRepository.FindByUserNameAndIsDeletedAsync(model.Username, 0.ToString());

                if (user != null && !string.IsNullOrEmpty(user.Username))
                {
                    var userRole = await _viewUserRoleRepository.FindByUserNameAsync(user.Username);
                    if (userRole != null)
                    {
                        var payload = new UserPayload
                        {
                            Id = user.Pk_Id,
                            Username = userRole.Username,
                            Email = user.Email,
                            DefaultRole = userRole.Data_Role,
                            DataRole = userRole.Data_Role,
                            FunctionRole = userRole.Function_Role,
                            ComponentRole = userRole.Component_Role,
                            Status = user.Status
                        };
                        SessionHelper.Set(HttpContext.Session, SESSION_KEYS.USER_PAYLOAD, payload);

                        var jwtConfig = JWTSetting.GetJwtSection(_configuration);
                        Log.Information($"JWT_Config: {JsonConvert.SerializeObject(jwtConfig, Formatting.Indented)}");
                        var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), jwtConfig.PrivateKey);
                        Log.Information($"JWT_Key_Path: {privateKeyPath}");
                        var privateKey = System.IO.File.ReadAllText(privateKeyPath);
                        Log.Information($"Private_Key: {privateKey}");

                        var jwtToken = new JwtSraTokenBuilder()
                            .AddSubject(jwtConfig.Subject)
                            .AddIssuer(jwtConfig.Issuer)
                            .AddAudience(jwtConfig.Audience)
                            .AddExpiry(jwtConfig.ExpirationInMins)
                            .AddNotBefore(jwtConfig.NotBeforeInSeconds)
                            .AddClaims(new List<Claim>())
                            .Build();
                        Log.Information($"Jwt_Sra_Token_Builder: {JsonConvert.SerializeObject(jwtToken, Formatting.Indented)}");

                        // create new token
                        var token = TokenProducer.Token(SessionHelper.Get<UserPayload>(HttpContext.Session, SESSION_KEYS.USER_PAYLOAD), privateKey, jwtToken);

                        // set token into cookie
                        SetTokenCookie(jwtConfig.JwtTokenCookie, token);

                        // results
                        return Ok(new
                        {
                            username = model.Username,
                            token,
                            expires = DateTime.Now.AddMinutes(jwtConfig.ExpirationInMins)
                        });
                    }
                }

                throw new HttpErrorException(HttpStatusCode.BadGateway, nameof(HttpStatusCode.BadGateway), ErrorMessages.SE003);
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

        /// <summary>
        /// Refresh token if expiry
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userRepository.FindAsync(1);
            return Ok(user);
        }

        /// <summary>
        /// Revoke token when refresh
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult RevokeToken([FromBody] RevokeTokenDto model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (token != null)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        #endregion

        #region helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        #endregion
    }
}