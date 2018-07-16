using Hub256.Common;
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
        public static IServiceCollection AddSwaggerDiscovery(this IServiceCollection services, IConfiguration configuration, ICommonStartup startup)
        {
            var serviceInfo = startup.ServiceInfo;
            var identityUrl = configuration.GetEndpointOptions().IdentityUrl;
            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{typeof(SwaggerServiceCollectionExtensions).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(filePath);
                if (serviceInfo.UsesAuthentication) {
                    c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = $"{identityUrl}/connect/authorize",
                        TokenUrl = $"{identityUrl}/connect/token",
                        Scopes = new Dictionary<string, string>(serviceInfo.RequiredScopes.Select(x => new KeyValuePair<string, string>(x.Scope, x.DisplayName))),                                            
                    });
                }
                c.DescribeAllEnumsAsStrings();

                var provider = startup.BranchServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var info = new Info()
                    {
                        Title = serviceInfo.ServiceDisplayName,
                        Version = description.ApiVersion.ToString(),
                        Description = serviceInfo.ServiceDescription,
                        Contact = new Contact() { Name = "256.foundation", Email = "dev@256.foundation" },
                        //TermsOfService = "MerSoft",
                        License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
                    };

                    if (description.IsDeprecated)
                    {
                        info.Description += " This API version has been deprecated.";
                    }
                    c.SwaggerDoc(description.GroupName, info);
                    startup.ServiceInfo.ApiVersions.Add(description.GroupName);
                }
                //c.OperationFilter<ApiVersionParameter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<SwaggerDefaultValues>();
                //c.OperationFilter<AddFileParamTypes>();

            });
            return services;
        }
    }
}