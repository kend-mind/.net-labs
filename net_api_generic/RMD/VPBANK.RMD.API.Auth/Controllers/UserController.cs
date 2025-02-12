using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using AutoMapper;
using System.Net;
using System;
using Serilog;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Services.Auth.Interfaces;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Data.Auth.Entities.POCOs;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Controllers;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.API.Common.Helpers.Requests;
using VPBANK.RMD.EFCore.Entities.Commons;

namespace VPBANK.RMD.API.Controllers
{
    public class UserController : GenericController<AuthContext, User, int>
    {
        protected readonly RequestHandler _requestHandler;
        protected readonly IUserRepository _userRepository;
        protected readonly IUserService _userService;

        public UserController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            IRabbitMqPublisher rabbitManager,
            ISubscriberInfoRepository subscriberRepository,

            IUnitOfWork<AuthContext> unitOfWork,
            ITrackableRepository<AuthContext, User, int> trackableRepository,
            IGenericRepository<AuthContext, User, int> genericRepository,

            RequestHandler requestHandler,
            IUserRepository userRepository,
            IUserService userService) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository,
                unitOfWork, trackableRepository, genericRepository)
        {
            _requestHandler = requestHandler;
            _userRepository = userRepository;
            _userService = userService;
        }

        /// <summary>
        /// Find User by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet(template: "{username}")]
        public virtual ActionResult<User> FindByUsername([Required][NotNull][FromRoute] string username)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _userService.FindByUsername(username);

                return Ok(entity);
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
        /// Find all username is supper administrator
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult<List<string>> FindBySupperAdmin()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usernames = _userService.FindBySupperAdmin();

                return Ok(usernames);
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
        /// Find all emails by all username
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult<List<string>> FindEmailsByUsernames([Required][NotNull][FromBody] List<string> usernames)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                return Ok(_userService.FindEmailsByUsernames(usernames));
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
        /// Delete user by pk_id
        /// </summary>
        /// <param name="pk_Id"></param>
        /// <returns></returns>
        [HttpDelete(template: "{pk_Id}")]
        public virtual IActionResult DeleteUser([Required][NotNull][FromRoute] int pk_Id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _userService.DeleteUserById(pk_Id);
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
        /// Find all usernames for selected_item
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ActionResult<List<SelectedItem>>> FindUsernames()
        {
            try
            {
                return Ok(await _userService.FindUsernames());
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
        /// Get all usernames not be grant roles (selected_item)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult<List<SelectedItem>> FindByUserNonRole()
        {
            try
            {
                return Ok(_userService.FindByUserNonRole());
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