using Hub256.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDiscovery(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetEndpointOptions().IdentityUrl;
            services.AddSwaggerGen(c =>
            {
                //TODO: Add correct scopes and service name
                
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{typeof(SwaggerServiceCollectionExtensions).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(filePath);
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{identityUrl}/connect/authorize",
                    TokenUrl = $"{identityUrl}/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        {  "loyaltyservices", "Loyalty services API"  },
                        //{ "openid", "Open ID required scope" },

                    },
                });
                c.DescribeAllEnumsAsStrings();
                //TODO: resolve api version without building service provider

                //var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                //foreach (var description in provider.ApiVersionDescriptions)
                //{
                var info = new Info()
                {
                    //Title = $"Loyalty Services API {description.ApiVersion}",
                    Title = $"Loyalty Services API Version 1",
                    Version = "v1",// description.ApiVersion.ToString(),
                    Description = "The program virtual card type ",
                    Contact = new Contact() { Name = "256.foundation", Email = "dev@256.foundation" },
                    //TermsOfService = "MerSoft",
                    License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
                };

                //if (description.IsDeprecated)
                //{
                //    info.Description += " This API version has been deprecated.";
                //}

                //c.SwaggerDoc(description.GroupName, info);
                c.SwaggerDoc("v1", info);
                //}
                c.OperationFilter<ApiVersionParameter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                //c.OperationFilter<AddFileParamTypes>();

            });
            return services;
        }
    }
}
