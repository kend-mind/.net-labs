using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.API.Common.Features
{
    public static class ApiVersioningConfigurator
    {
        public static void ConfigureApiVersioningFeature(this IServiceCollection services, ApiSwaggerInfoSection _apiSwaggerInfo)
        {
            // Add API Versioning to as service to your project 
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version
                string[] version = _apiSwaggerInfo.Version.Split(SpecificCharacteristics.DOT);
                config.DefaultApiVersion = new ApiVersion(int.Parse(version[0]), int.Parse(version[1]));

                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;

                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;

                // DEFAULT Version reader is QueryStringApiVersionReader();  
                // clients request the specific version using the X-version header
                config.ApiVersionReader = new HeaderApiVersionReader("X-version");

                // Supporting multiple versioning scheme
                config.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("vpb-rmd-api-version"), new QueryStringApiVersionReader("api-version"));
            });
        }
    }
}
