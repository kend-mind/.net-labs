using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.Utils.Common;

namespace VPBANK.RMD.API.Common.Features
{
    public static class SwaggerOptionsConfigurator
    {
        internal static bool IsSwaggerAdded { get; set; }
        internal static bool IsSwaggerSecurityUsed { get; set; }

        public static IServiceCollection ConfigureSwaggerFeature(this IServiceCollection services, ApiSwaggerInfoSection _apiSwaggerInfo)
        {
            if (!IsSwaggerAdded)
            {
                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc($"v{_apiSwaggerInfo.Version}", new OpenApiInfo
                    {
                        Version = _apiSwaggerInfo.Version,
                        Title = _apiSwaggerInfo.Title,
                        Description = _apiSwaggerInfo.Description,
                        TermsOfService = new Uri(_apiSwaggerInfo.TermsOfService),
                        Contact = new OpenApiContact
                        {
                            Name = _apiSwaggerInfo.Contact.Name,
                            Email = _apiSwaggerInfo.Contact.Email,
                            Url = new Uri(_apiSwaggerInfo.Contact.Url)
                        },
                        License = new OpenApiLicense
                        {
                            Name = _apiSwaggerInfo.License.Name,
                            Url = new Uri(_apiSwaggerInfo.License.Url)
                        }
                    });

                    // Set the comments path for the Swagger JSON and UI. {Assembly.GetExecutingAssembly().GetName().Name}.xml
                    var xmlFile = _apiSwaggerInfo.ApiDocument;
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    // API_KEY
                    c.AddSecurityDefinition(ApiKeys.X_AUTH_TOKEN, new OpenApiSecurityScheme
                    {
                        Description = $"Api key needed to access the endpoints. X-API-KEY: {ApiKeys.X_AUTH_TOKEN}",
                        In = ParameterLocation.Header,
                        Name = ApiKeys.X_AUTH_TOKEN,
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = ApiKeys.X_AUTH_TOKEN,
                                Type = SecuritySchemeType.ApiKey,
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = ApiKeys.X_AUTH_TOKEN }
                            },
                            new string[] {}
                        }
                    });
                });

                IsSwaggerAdded = true;
            }

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, ApiSwaggerInfoSection _apiSwaggerInfo)
        {
            if (!IsSwaggerSecurityUsed)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        string.Format(_apiSwaggerInfo.Endpoint.EndpointUrl, _apiSwaggerInfo.Version),
                        string.Format(_apiSwaggerInfo.Endpoint.EndpointSelectDrop, _apiSwaggerInfo.Version));
                    c.DocExpansion(DocExpansion.None);

                    c.DocumentTitle = _apiSwaggerInfo.Title;
                    c.RoutePrefix = _apiSwaggerInfo.Endpoint.RoutePrefix;
                });

                // Add docs
                app.UseReDoc(c =>
                {
                    c.SpecUrl($"/v{_apiSwaggerInfo.Version}/swagger.json");
                    c.EnableUntrustedSpec();
                    c.ScrollYOffset(10);
                    c.HideHostname();
                    c.HideDownloadButton();
                    c.ExpandResponses("200, 201");
                    c.RequiredPropsFirst();
                    c.NoAutoAuth();
                    c.PathInMiddlePanel();
                    c.HideLoading();
                    c.NativeScrollbars();
                    c.DisableSearch();
                    c.OnlyRequiredInSamples();
                    c.SortPropsAlphabetically();
                });

                IsSwaggerSecurityUsed = true;
            }

            return app;
        }
    }
}
