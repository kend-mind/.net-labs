using Microsoft.Extensions.Configuration;
using static VPBANK.RMD.API.Settings.AppSettings;

namespace VPBANK.RMD.API.Settings
{
    public static class SwaggerSetting
    {
        public static ApiSwaggerInfoSection GetSwaggerInfoSection(IConfiguration configuration)
        {
            ApiSwaggerInfoSection _apiSwaggerInfo = new ApiSwaggerInfoSection();

            // get config
            configuration.GetSection(nameof(AppSettings.ApiSwaggerInfo)).Bind(_apiSwaggerInfo);
            var swaggerContact = new ApiSwaggerContact();
            configuration.GetSection($"{nameof(AppSettings.ApiSwaggerInfo)}:{nameof(ApiSwaggerInfoSection.Contact)}").Bind(swaggerContact);
            _apiSwaggerInfo.Contact = swaggerContact;
            var swaggerLicense = new ApiSwaggerLicense();
            configuration.GetSection($"{nameof(AppSettings.ApiSwaggerInfo)}:{nameof(ApiSwaggerInfoSection.License)}").Bind(swaggerLicense);
            _apiSwaggerInfo.License = swaggerLicense;
            var apiSwaggerEndpoint = new ApiSwaggerEndpoint();
            configuration.GetSection($"{nameof(AppSettings.ApiSwaggerInfo)}:{nameof(ApiSwaggerInfoSection.Endpoint)}").Bind(apiSwaggerEndpoint);
            _apiSwaggerInfo.Endpoint = apiSwaggerEndpoint;

            return _apiSwaggerInfo;
        }
    }
}
