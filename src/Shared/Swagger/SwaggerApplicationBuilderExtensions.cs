using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDiscovery(this IApplicationBuilder app)
        {
            var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger(o =>
            {
                o.RouteTemplate= ".well-known/api-docs/{documentName}/swagger.json";
            });
               //.UseSwaggerUI(c =>
               //{
               //    foreach (var description in apiProvider.ApiVersionDescriptions)
               //    {
               //        c.SwaggerEndpoint(
               //            $"/swagger/{description.GroupName}/swagger.json",
               //            description.GroupName.ToUpperInvariant());
               //    }
               //    c.OAuthClientId("loyaltyservicesswaggerui");
               //});

            return app;
        }
    }
}
