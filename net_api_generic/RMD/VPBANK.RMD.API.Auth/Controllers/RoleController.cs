using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using AutoMapper;
using System;
using System.Net;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Services.Auth.Interfaces;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Controllers;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.API.Common.Helpers.Requests;
using VPBANK.RMD.Data.Auth.Entities.Views;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Services.Auth.DataContracts;

namespace VPBANK.RMD.API.Controllers
{
    public class RoleController : QueryController<AuthContext, ViewUserRole, long>
    {
        protected readonly RequestHandler _requestHandler;
        protected readonly IUserRepository _userRepository;
        protected readonly IUserService _userService;
        protected readonly IRoleService _roleService;

        public RoleController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            IRabbitMqPublisher rabbitManager,
            ISubscriberInfoRepository subscriberRepository,

            IUnitOfWork<AuthContext> unitOfWork,
            ITrackableRepository<AuthContext, ViewUserRole, long> trackableRepository,
            IGenericRepository<AuthContext, ViewUserRole, long> genericRepository,

            RequestHandler requestHandler,
            IUserRepository userRepository,
            IUserService userService,
            IRoleService roleService) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository,
                unitOfWork, trackableRepository, genericRepository)
        {
            _requestHandler = requestHandler;

            _userRepository = userRepository;
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Get all data roles
        /// </summary>
        /// <returns>List selected items (SelectedItem)</returns>
        [HttpGet]
        public IActionResult GuiDataRoles()
        {
            return Ok(_roleService.GuiDataRoles());
        }

        /// <summary>
        /// Save user with data role, func role, component role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>object UserRoleMappingDto</returns>
        [HttpPost]
        public virtual ActionResult<UserRoleMappingDto> Save([Required][NotNull][FromBody] UserRoleMappingDto entity)
        {
            try
            {
                // check object valid
                if (!ModelState.IsValid || entity == null)
                    return BadRequest(ModelState);

                // insert data
                _roleService.Save(entity);

                // results
                return CreatedAtAction(nameof(Save), new { entity.Username }, entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw ex;
                else
                    throw new HttpErrorException(HttpStatusCode.BadRequest, nameof(HttpStatusCode.BadRequest), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
                // out method
            }
        }

        /// <summary>
        /// Delete all roles by username
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Reset([Required][NotNull][FromBody] UserAuthDto entity)
        {
            try
            {
                // check object valid
                if (!ModelState.IsValid || entity == null)
                    return BadRequest(ModelState);

                // insert data
                _roleService.DeleteRoleByUsername(entity.Username);

                // results
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw ex;
                else
                    throw new HttpErrorException(HttpStatusCode.BadRequest, nameof(HttpStatusCode.BadRequest), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
                // out method
            }
        }

        /// <summary>
        /// Delete func role by username
        /// </summary>
        /// <param name="funcRole"></param>
        /// <returns></returns>
        [HttpDelete(template: "{funcRole}")]
        public virtual IActionResult DeleteFuncRole([Required][NotNull][FromRoute] string funcRole)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _roleService.DeleteFunctionRole(funcRole);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw ex;
                else
                    throw new HttpErrorException(HttpStatusCode.BadRequest, nameof(HttpStatusCode.BadRequest), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }

        /// <summary>
        /// Delete component role (set IsDeleted = true)
        /// </summary>
        /// <param name="compRole"></param>
        /// <returns></returns>
        [HttpDelete(template: "{compRole}")]
        public virtual IActionResult DeleteCompRole([Required][NotNull][FromRoute] string compRole)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _roleService.DeleteComponentRole(compRole);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw ex;
                else
                    throw new HttpErrorException(HttpStatusCode.BadRequest, nameof(HttpStatusCode.BadRequest), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }
    }
}