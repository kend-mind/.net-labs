using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using VPBANK.RMD.API.Common.Extensions;
using VPBANK.RMD.API.Common.Helpers.Requests;
using VPBANK.RMD.API.Common.Features;
using VPBANK.RMD.API.Common.Securities.ApiKey;
using VPBANK.RMD.API.Common.Transformers;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Helpers.Mapping;
using VPBANK.RMD.API.Logs;
using VPBANK.RMD.API.Logs.Helpers;

namespace VPBANK.RMD.API.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ApiSwaggerInfoSection _apiSwaggerInfo;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _apiSwaggerInfo = SwaggerSetting.GetSwaggerInfoSection(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // other configs
            services.AddLogging();
            services.AddSingleton<ILoggerManager, NLogManager>();
            // Data mapper profiler setting
            services.AddAutoMapper(typeof(ModelMappingProfile).Assembly);
            services.AddSingleton(Log.Logger);
            services.AddCacheConfiguration(Configuration);
            services.AddHttpClient();

            // Mvc, Controller, JsonOptions
            services.ConfigureBaseOptionsFeature();

            // Url Helper
            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            // add settings
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAppSettingsReader, AppSettingsReader>();

            // Request header
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // RabbitMq
            services
                .AddConfRabbitMQ(Configuration)
                .AddPublicerRabbitMQ(Configuration);
            // ELK
            services.AddElasticSearchConf(Configuration);

            // Session
            services.AddSession(options =>
            {
                // Set a short timeout
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.AddSingleton<RequestHandler>();
            services.AddHttpContextAccessor();

            // Add API Versioning to as service to your project 
            services.ConfigureApiVersioningFeature(_apiSwaggerInfo: _apiSwaggerInfo);

            // Applies a parameter transformer to all attribute routes in an application.
            // Customizes the attribute route token values as they are replaced.
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.ConfigureSwaggerFeature(_apiSwaggerInfo: _apiSwaggerInfo);

            // JWT config
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
            .AddApiKeySraJose(options => { });

            // Enable the use of an [Authorize] attribute on methods and classes to protect
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.OnlyAdministrators, policy => policy.Requirements.Add(new OnlyAdministratorsRequirement()));
                options.AddPolicy(Policies.OnlyModerators, policy => policy.Requirements.Add(new OnlyModeratorsRequirement()));
                options.AddPolicy(Policies.OnlyUsers, policy => policy.Requirements.Add(new OnlyUsersRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, OnlyAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ModeratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OnlyUsersAuthorizationHandler>();

            // Data Context
            services.ConfigureBusinessFeatures(_configuration: Configuration);

            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var pathBase = Configuration[Utils.Common.Shared.Constants.PATH_BASE];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseCors();
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            // Custom errors
            app.ConfigureErrorExceptionMiddleware();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerDocumentation(_apiSwaggerInfo: _apiSwaggerInfo);

            loggerFactory.AddSerilog();
            // Adding additional data to the Serilog request log
            app.UseSerilogRequestLogging(option => option.EnrichDiagnosticContext = LogHelper.EnrichFromRequest);

            // Call an ASP.NET Core web API with JavaScript
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSession();

            // Initilize Rabbit Listener in ApplicationBuilderExtentions
            //app.UseRabbitListener();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
