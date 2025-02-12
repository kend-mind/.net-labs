using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using VPBANK.RMD.API.Common.Filters;
using VPBANK.RMD.API.Common.Resolvers;
using VPBANK.RMD.API.Common.Helpers;
using static VPBANK.RMD.Utils.Common.Enums;

namespace VPBANK.RMD.API.Common.Features
{
    public static class BaseOptionsConfigurator
    {
        public static IServiceCollection ConfigureBaseOptionsFeature(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    // Defaults to 8192
                    options.MaxIAsyncEnumerableBufferLimit = 1000000000/*'N'*/;
                    options.AllowEmptyInputInBodyModelBinding = true;
                });

            services
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = true;

                    // an instance
                    options.Filters.Add(new AddHeaderAttribute("GlobalAddHeader", "Result filter added to MvcOptions.Filters"));
                    
                    // an type
                    options.Filters.Add(typeof(ActionFilterHandler));
                    options.Filters.Add(typeof(AsyncActionFilterHandler));
                    options.InputFormatters.Insert(0, new BinaryInputFormatter());

                    options.AllowEmptyInputInBodyModelBinding = true;
                    foreach (var formatter in options.InputFormatters)
                    {
                        if (formatter.GetType() == typeof(SystemTextJsonInputFormatter))
                            ((SystemTextJsonInputFormatter)formatter).SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeNames.Text.Plain));
                    }
                })
                .AddJsonOptions(options =>
                {
                    var jsonResolver = new JsonPropertyNamesContractResolver_v31
                    {
                        ResolverCase = ResolverCaseEnums.Camelize,
                        KeepUnderscores = true
                    };
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonResolver;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = jsonResolver;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

                    // json format response
                    // options.JsonSerializerOptions.Converters.Add(new DateTimeResolver());
                })
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            return services;
        }
    }
}
